using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class EventDetails : Details
    {
        public override AdvertisementType Type => AdvertisementType.Events;
        public string EventType { get; private set; }
        public DateTime EventDate { get; private set; }
        public string Location { get; private set; }

        public EventDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string eventType, DateTime eventDate, string location) : base(title, description, price, images)
        {
            EventType = eventType;
            EventDate = eventDate;
            Location = location;
        }
    }

}
