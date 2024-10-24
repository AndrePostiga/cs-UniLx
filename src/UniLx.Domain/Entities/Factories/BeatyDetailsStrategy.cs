using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Domain.Entities.Factories
{
    internal class BeatyDetailsStrategy : DetailsFactoryStrategy
    {
        public AdvertisementType Type => AdvertisementType.Beauty;

        public Details CreateDetails(string title, string? description, int? price)
        {
            throw new NotImplementedException();
        }
    }
}
