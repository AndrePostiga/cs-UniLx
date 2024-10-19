using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class FashionDetails : Details
    {
        public override AdvertisementType Type => AdvertisementType.Fashion;
        public string ClothingType { get; private set; }
        public string Brand { get; private set; }
        public string Size { get; private set; }

        public FashionDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string clothingType, string brand, string size) : base(title, description, price, images)
        {
            ClothingType = clothingType;
            Brand = brand;
            Size = size;
        }
    }

}
