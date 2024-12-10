using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class FashionSize : SmartEnum<FashionSize>
    {
        public static readonly FashionSize PP = new FashionSize("pp".ToSnakeCase(), 1);  // Extra Small
        public static readonly FashionSize P = new FashionSize("p".ToSnakeCase(), 2);   // Small
        public static readonly FashionSize M = new FashionSize("m".ToSnakeCase(), 3);   // Medium
        public static readonly FashionSize G = new FashionSize("g".ToSnakeCase(), 4);   // Large
        public static readonly FashionSize GG = new FashionSize("gg".ToSnakeCase(), 5); // Extra Large
        public static readonly FashionSize XG = new FashionSize("xg".ToSnakeCase(), 6); // Extra Extra Large
        public static readonly FashionSize XXG = new FashionSize("xxg".ToSnakeCase(), 6); // Extra Extra Large
        public static readonly FashionSize XXXG = new FashionSize("xxxg".ToSnakeCase(), 6); // Extra Extra Large
        public static readonly FashionSize PlusSize = new FashionSize("PlusSize".ToSnakeCase(), 6); // Extra Extra Large

        private FashionSize(string name, int value) : base(name, value) { }
    }
}
