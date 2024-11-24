using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class EmploymentType : SmartEnum<EmploymentType>
    {
        public static readonly EmploymentType FullTime = new EmploymentType("FullTime".ToSnakeCase().ToLower(), 1);
        public static readonly EmploymentType PartTime = new EmploymentType("PartTime".ToSnakeCase().ToLower(), 2);
        public static readonly EmploymentType Contract = new EmploymentType("Contract".ToSnakeCase().ToLower(), 3);

        private EmploymentType(string name, int value) : base(name, value) { }
    }
}
