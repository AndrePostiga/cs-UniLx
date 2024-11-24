using UniLx.Application.Usecases.SharedModels.Responses;

namespace UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse
{
    public class EventsDetailsResponse : BaseDetailsResponse
    {
        public string? EventType { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Organizer { get; set; }
        public string? DressCode { get; set; }
        public List<string>? Highlights { get; set; }
        public bool IsOnline { get; set; }
        public ContactInformationResponse? ContactInfo { get; set; }
        public string? AgeRestriction { get; set; }
    }
}
