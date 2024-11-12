using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.Seedwork.ValueObj
{
    public record Image
    {
        public Uri Url { get; }
        public string Format { get; }
        public long SizeInBytes { get; }
        public int Width { get; }
        public int Height { get; }
        public string? Alt { get; }
        public string? Target { get; }

        private static readonly string[] SupportedFormats = { "jpg", "jpeg", "png", "bmp" };
        private static readonly long MaxFileSizeInBytes = 5 * 1024 * 1024; // 5 MB max size
        private static readonly int MaxWidth = 1280;  // Maximum allowed width
        private static readonly int MaxHeight = 720;  // Maximum allowed height
        private static readonly int MaxAlt = 256;  // Maximum Alt Text
        private static readonly string[] ValidTargets = { "_self", "_blank", "_parent", "_top" };

        public Image(Uri url, string format, long sizeInBytes, int width, int height, string? alt, string? target)
        {
            DomainException.ThrowIf(url == null || !url.IsAbsoluteUri, "Url must be a valid absolute URI.");
            DomainException.ThrowIf(Array.IndexOf(SupportedFormats, format.ToLower()) == -1, $"Unsupported image format: {format}. Supported formats are: {string.Join(", ", SupportedFormats)}");
            DomainException.ThrowIf(sizeInBytes <= 0, "File size must be greater than zero.");
            DomainException.ThrowIf(sizeInBytes > MaxFileSizeInBytes, $"File size exceeds the maximum limit of {MaxFileSizeInBytes / (1024 * 1024)} MB.");
            DomainException.ThrowIf(width <= 0 || height <= 0, "Image dimensions must be positive.");
            DomainException.ThrowIf(width > MaxWidth || height > MaxHeight, $"Image dimensions exceed the maximum allowed resolution of {MaxWidth}x{MaxHeight} pixels.");
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(alt), $"Image dimensions exceed the maximum allowed resolution of {MaxWidth}x{MaxHeight} pixels.");

            if (!string.IsNullOrWhiteSpace(alt))
                DomainException.ThrowIf(alt.Length > MaxAlt, $"Alt text must has {MaxAlt} characters or less");

            if (!string.IsNullOrWhiteSpace(target))
                DomainException.ThrowIf(Array.IndexOf(ValidTargets, target) == -1, $"Invalid target value: {target}. Valid targets are: {string.Join(", ", ValidTargets)}");

            Url = url!;
            Format = format;
            SizeInBytes = sizeInBytes;
            Width = width;
            Height = height;
            Alt = alt;
            Target = target;
        }
    }
}
