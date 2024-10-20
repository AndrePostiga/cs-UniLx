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

        public static StorageImage CreatePublicImage(string accessUrl) => new StorageImage(StorageType.DirectUrl, false, accessUrl);
        public static StorageImage CreatePrivateImage(string path) => new StorageImage(StorageType.Private, true, path: path);
        public static StorageImage CreateImageWithPath(bool isPrivate, string path) => new StorageImage(StorageType.Path, isPrivate, path: path);

        private StorageImage(StorageType storageType, bool isPrivate, string? accessUrl=null, string? path=null)
        {
            StorageType = storageType;
            IsPrivate = isPrivate;
            Path = path;

            if (StorageType == StorageType.Path || StorageType == StorageType.Private)
                ValidateImagePath(path);

            if (StorageType == StorageType.DirectUrl)
                ValidateAndSetAccessUrl(accessUrl);
        }

        private void ValidateImagePath(string? path)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(path), "Path cannot be null or empty for Path or Private storage.");            

            var fileExtension = System.IO.Path.GetExtension(path)?.ToLower().TrimStart('.');
            DomainException.ThrowIf(!Array.Exists(SupportedImageFormats, format => format == fileExtension),
                $"Unsupported image format: {fileExtension}. Supported formats are: {string.Join(", ", SupportedImageFormats)}");
        }

        private void ValidateAndSetAccessUrl(string? accessUrl)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(accessUrl), "AccessUrl is required for direct URLs.");

            if (!Uri.TryCreate(accessUrl, UriKind.Absolute, out var uriResult) || (uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps))
                throw new DomainException("AccessUrl must be a valid absolute URL (HTTP or HTTPS).");

            AccessUrl = uriResult;
        }
    }
}
