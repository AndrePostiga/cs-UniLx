using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class RealEstateDetails : Details
    {
        public override AdvertisementType Type => AdvertisementType.RealEstate;
        public string Address { get; private set; }
        public double SquareFootage { get; private set; }
        public int Bedrooms { get; private set; }

        public RealEstateDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string address, double squareFootage, int bedrooms) : base(title, description, price, images)
        {
            Address = address;
            SquareFootage = squareFootage;
            Bedrooms = bedrooms;
        }
    }

}
