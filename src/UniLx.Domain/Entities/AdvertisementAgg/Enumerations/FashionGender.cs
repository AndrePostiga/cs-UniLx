using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class FashionGender : SmartEnum<FashionGender>
    {
        public static readonly FashionGender Male = new FashionGender("Male".ToSnakeCase().ToLower(), 1);
        public static readonly FashionGender Female = new FashionGender("Female".ToSnakeCase().ToLower(), 2);
        public static readonly FashionGender Unisex = new FashionGender("Unisex".ToSnakeCase().ToLower(), 3);

        private FashionGender(string name, int value) : base(name, value) { }
    }
}
