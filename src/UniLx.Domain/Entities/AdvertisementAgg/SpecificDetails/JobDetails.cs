using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class JobDetails : Details
    {
        public override AdvertisementType Type => AdvertisementType.JobOpportunities;
        public string Position { get; private set; }
        public string Company { get; private set; }
        public double Salary { get; private set; }

        public JobDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string position, string company, double salary) : base(title, description, price)
        {
            Position = position;
            Company = company;
            Salary = salary;
        }
    }
}
