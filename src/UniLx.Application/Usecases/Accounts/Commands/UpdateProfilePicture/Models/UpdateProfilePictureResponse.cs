namespace UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture.Models
{
    internal class UpdateProfilePictureResponse
    {
        public string? ProfilePictureUrl { get; set; }

        public DateTime? UrlExpiresAt { get; set; }

        public UpdateProfilePictureResponse(string? profilePicturePath, DateTime? urlExpiresAt)
        {
            ProfilePictureUrl = profilePicturePath;
            UrlExpiresAt = urlExpiresAt;
        }
    }
}
