namespace UniLx.Application.Usecases.Advertisements.Commands.AdvertisementGeneratePresignUrl.Models
{
    public class AdvertisementPresignUrlResponse
    {
        public string SignedUrl { get; set; }
        public string FileName { get; set; }
        public DateTime UrlExpiresAt { get; set; }

        public AdvertisementPresignUrlResponse(string signedUrl, string fileName, DateTime urlExpiresAt)
        {
            SignedUrl = signedUrl;
            FileName = fileName;
            UrlExpiresAt = urlExpiresAt;
        }
    }
}
