namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request.DetailsRequest
{
    public class EventDetailsRequest
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? EventType { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Organizer { get; set; }
        public string? AgeRestriction { get; set; }
        public string? DressCode { get; set; }
        public List<string>? Highlights { get; set; }
        public bool? IsOnline { get; set; }
        public ContactInformationRequest? ContactInformation { get; set; }
    }
}