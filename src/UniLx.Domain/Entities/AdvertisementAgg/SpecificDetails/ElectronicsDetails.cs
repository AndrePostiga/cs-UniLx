using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class ElectronicsDetails : Details
    {
        public override AdvertisementType Type => AdvertisementType.Electronics;
        public string ProductType { get; private set; }
        public string Brand { get; private set; }
        public bool IsNew { get; private set; }

        public ElectronicsDetails(
            string title, string description, int price, IEnumerable<Image> images, 
            string productType, string brand, bool isNew) : base(title, description, price, images)
        {
            ProductType = productType;
            Brand = brand;
            IsNew = isNew;
        }
    }

}
