using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class PropertyType : SmartEnum<PropertyType>
    {
        public static readonly PropertyType Residential = new PropertyType(nameof(Residential).ToSnakeCase(), 1);
        public static readonly PropertyType Commercial = new PropertyType(nameof(Commercial).ToSnakeCase(), 2);
        public static readonly PropertyType Land = new PropertyType(nameof(Land).ToSnakeCase(), 3);

        public PropertyType(string name, int value) : base(name, value) {}
    }
}
