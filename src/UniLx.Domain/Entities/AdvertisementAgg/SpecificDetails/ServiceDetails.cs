using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class ServiceDetails : Details
    {
        public override AdvertisementType Type => AdvertisementType.Services;
        public string ServiceType { get; private set; }
        public double HourlyRate { get; private set; }

        public ServiceDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string serviceType, double hourlyRate) : base(title, description, price)
        {
            ServiceType = serviceType;
            HourlyRate = hourlyRate;
        }
    }
}
