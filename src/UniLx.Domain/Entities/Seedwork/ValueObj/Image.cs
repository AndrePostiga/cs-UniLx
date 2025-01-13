using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.Seedwork.ValueObj
{
    public class Image
    {
        private static readonly string[] SupportedImageFormats = { "jpg", "jpeg", "png", "bmp" };

        public string OriginalFileName { get; private set; }
        public string FileName { get; private set; }
        public string Extension { get; private set; }

        private Image() { }

        public static Image Create(string originalFileName)
        {
            ValidateFileName(originalFileName);

            var extension = Path.GetExtension(originalFileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";

            return new Image(originalFileName, uniqueFileName, extension);
        }

        private Image(string originalFileName, string fileName, string extension)
        {
            OriginalFileName = originalFileName;
            FileName = fileName;
            Extension = extension;
        }

        public static void ValidateFileName(string fileName)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(fileName), "File name cannot be null or empty.");
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            DomainException.ThrowIf(!ValidateExtension(extension), $"Invalid file format: {extension}. Supported formats are: {string.Join(", ", SupportedImageFormats)}");
        }

        public static bool ValidateExtension(string extension)
        {
            if (string.IsNullOrWhiteSpace(extension))
                return false;

            extension = extension.ToLowerInvariant().TrimStart('.');
            return Array.Exists(SupportedImageFormats, format => format == extension);
        }
    }
}