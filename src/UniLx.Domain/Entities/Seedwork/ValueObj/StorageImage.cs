using Ardalis.SmartEnum;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.Seedwork.ValueObj
{
    public sealed class StorageType : SmartEnum<StorageType>
    {
        public static readonly StorageType DirectUrl = new StorageType(nameof(DirectUrl).ToLower(), 1);

        public static readonly StorageType Path = new StorageType(nameof(Path).ToLower(), 2);

        public static readonly StorageType Private = new StorageType(nameof(Private).ToLower(), 3);

        private StorageType(string name, int value) : base(name, value) { }
    }

    public class StorageImage
    {
        private static readonly string[] SupportedImageFormats = { "jpg", "jpeg", "png", "bmp" };

        public StorageType StorageType { get; private set; }
        public bool IsPrivate { get; private set; }
        public Uri? AccessUrl { get; private set; }
        public string? Path { get; private set; }
        public string? FileName { get; private set; }
        public string? FullPath => Path != null && FileName != null ? $"{Path}/{FileName}" : null;

        public static StorageImage CreatePublicImage(string? accessUrl) => new StorageImage(StorageType.DirectUrl, false, accessUrl);
        public static StorageImage CreatePrivateImage(string path, string fileName) => new StorageImage(StorageType.Private, true, path: path, fileName: fileName);
        public static StorageImage CreateImageWithPath(bool isPrivate, string path, string fileName) => new StorageImage(StorageType.Path, isPrivate, path: path, fileName: fileName);

        private StorageImage() { }

        private StorageImage(StorageType storageType, bool isPrivate, string? accessUrl = null, string? path = null, string? fileName = null)
        {
            StorageType = storageType;
            IsPrivate = isPrivate;
            Path = path;
            FileName = fileName;

            if (StorageType == StorageType.Path || StorageType == StorageType.Private)
            {
                DomainException.ThrowIf(string.IsNullOrWhiteSpace(path), "Path cannot be null or empty for Path or Private storage.");
                DomainException.ThrowIf(string.IsNullOrWhiteSpace(fileName), "FileName cannot be null or empty for Path or Private storage.");
                DomainException.ThrowIf(!ValidateImageFileName(fileName), $"Invalid file format: {fileName}. Supported formats are: {string.Join(", ", SupportedImageFormats)}");
            }

            if (StorageType == StorageType.DirectUrl)
            {
                ValidateAndSetAccessUrl(accessUrl);
            }
        }

        public static bool ValidateImageFileName(string? fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return false;

            var fileExtension = System.IO.Path.GetExtension(fileName)?.ToLower().TrimStart('.');
            return Array.Exists(SupportedImageFormats, format => format == fileExtension);
        }

        private void ValidateAndSetAccessUrl(string? accessUrl)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(accessUrl), "AccessUrl is required for direct URLs.");

            if (!Uri.TryCreate(accessUrl, UriKind.Absolute, out var uriResult) || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
            {
                throw new DomainException("AccessUrl must be a valid absolute URL (HTTP or HTTPS).");
            }

            AccessUrl = uriResult;
        }
    }
}
