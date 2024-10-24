using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class ToyDetails : Details
    {
        public override AdvertisementType Type => AdvertisementType.Toys;

        public string ToyType { get; private set; }
        public string AgeRange { get; private set; }
        public bool IsBatteryOperated { get; private set; }

        public ToyDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string toyType, string ageRange, bool isBatteryOperated) : base(title, description, price)
        {
            ToyType = toyType;
            AgeRange = ageRange;
            IsBatteryOperated = isBatteryOperated;
        }
    }
}
