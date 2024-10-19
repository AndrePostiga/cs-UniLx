using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class BeautyDetails : Details
    {
        public override AdvertisementType Type => AdvertisementType.Beauty;

        public string ProductType { get; private set; }
        public string Brand { get; private set; }
        public string SkinType { get; private set; }

        public BeautyDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string productType, string brand, string skinType) : base(title, description, price, images)
        {
            ProductType = productType;
            Brand = brand;
            SkinType = skinType;
        }
    }

}
