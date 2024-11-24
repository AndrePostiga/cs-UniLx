using Ardalis.SmartEnum;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class FashionGender : SmartEnum<FashionGender>
    {
        public static readonly FashionGender Male = new FashionGender("Male".ToLower(), 1);
        public static readonly FashionGender Female = new FashionGender("Female".ToLower(), 2);
        public static readonly FashionGender Unisex = new FashionGender("Unisex".ToLower(), 3);

        private FashionGender(string name, int value) : base(name, value) { }
    }
}
