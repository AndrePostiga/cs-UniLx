using UniLx.Shared.Abstractions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Events
{
    public class CreateAdvertisementEvent : Event
    {
        public CreateAdvertisementEventModel Content { get; private set; }
        
        internal const string ADVERTISEMENT_CREATED = "advertisements.created";

        private CreateAdvertisementEvent(Advertisement advertisement, string eventType) : base(eventType)
        {
            Content = new CreateAdvertisementEventModel(advertisement);
        }       

        public static CreateAdvertisementEvent AdvertisementCreated(Advertisement advertisement)
            => new(advertisement, ADVERTISEMENT_CREATED);
    }
}
