namespace UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture.Models
{
    internal class UpdateProfilePictureResponse
    {
        public string SignedUrl { get; set; }
        public string FileName { get; set; }
        public DateTime UrlExpiresAt { get; set; }

        public UpdateProfilePictureResponse(string signedUrl, string fileName, DateTime urlExpiresAt)
        {
            SignedUrl = signedUrl;
            FileName = fileName;
            UrlExpiresAt = urlExpiresAt;
        }
    }
}
