namespace UniLx.Application.Usecases.SharedModels.Responses
{
    public class OwnerSummaryResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Email { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
}
