using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class EventsDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.Events;

        public string EventType { get; private set; }
        public DateTime EventDate { get; private set; }
        public string? Organizer { get; private set; }            
        public AgeRestriction AgeRestriction { get; private set; }       
        public string? DressCode { get; private set; }            
        public List<string>? Highlights { get; private set; }     
        public bool IsOnline { get; private set; }                
        public ContactInformation ContactInfo { get; private set; }

        private EventsDetails() : base()
        { }

        public EventsDetails(
            string title, string? description, int? price, string eventType, 
            DateTime eventDate, string? organizer, string? ageRestriction, 
            string? dressCode, List<string>? highlights, bool isOnline, 
            ContactInformation? contactInformation)
            : base(title, description, price)
        {
            SetEventType(eventType);
            SetEventDate(eventDate);
            Organizer = organizer;
            SetAgeRestriction(ageRestriction);
            DressCode = dressCode;
            SetHighlights(highlights);
            SetContactInformation(contactInformation);
            IsOnline = isOnline;
        }

        private void SetContactInformation(ContactInformation? contactInformation)
        {
            DomainException.ThrowIf(contactInformation is null, "Events advertisements must has contact information.");
            ContactInfo = contactInformation!;
        }

        private void SetAgeRestriction(string? restriction)
        {
            var hasRestrictionType = AgeRestriction.TryFromName(restriction, ignoreCase: true, out var restrictionType);
            DomainException.ThrowIf(hasRestrictionType == false, $"Invalid restriction type, possible values are {string.Join(",", AgeRestriction.List)}.");
            AgeRestriction = restrictionType;
        }

        private void SetEventType(string eventType)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(eventType), "EventType cannot be null or empty.");
            DomainException.ThrowIf(eventType.Length > 100, "EventType must be 100 characters or less.");
            EventType = eventType;
        }

        private void SetEventDate(DateTime eventDate)
        {
            DomainException.ThrowIf(eventDate < DateTime.UtcNow, "EventDate cannot be in the past.");
            EventDate = eventDate;
        }

        private void SetHighlights(List<string>? highlights)
        {
            highlights = highlights?.Where(h => !string.IsNullOrWhiteSpace(h) && h.Length <= 100).ToList() ?? new List<string>();

            DomainException.ThrowIf(highlights.Any(h => string.IsNullOrWhiteSpace(h)), "Highlights cannot contain null or empty entries.");
            DomainException.ThrowIf(highlights.Any(h => h.Length > 100), "Each highlight must be 100 characters or less.");
            Highlights = highlights;
        }
    }
}
