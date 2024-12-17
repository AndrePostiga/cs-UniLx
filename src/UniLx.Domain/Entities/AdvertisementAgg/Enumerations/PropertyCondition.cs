using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class PropertyCondition : SmartEnum<PropertyCondition>
    {
        public static readonly PropertyCondition New = new PropertyCondition(nameof(New).ToSnakeCase(), 1);
        public static readonly PropertyCondition Renovated = new PropertyCondition(nameof(Renovated).ToSnakeCase(), 2);
        public static readonly PropertyCondition Used = new PropertyCondition(nameof(Used).ToSnakeCase(), 3);

        protected PropertyCondition(string name, int value) : base(name, value) { }
    }
}
