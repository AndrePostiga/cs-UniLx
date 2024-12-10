using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class VehicleDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.Vehicles;
        public string Brand { get; private set; }
        public string Model { get; private set; }
        public int Year { get; private set; }

        private VehicleDetails() { }
        public VehicleDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string brand, string model, int year) : base(title, description, price)
        {
            Brand = brand;
            Model = model;
            Year = year;
        }
    }
}
