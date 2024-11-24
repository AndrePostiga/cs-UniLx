using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class FashionSize : SmartEnum<FashionSize>
    {
        public static readonly FashionSize PP = new FashionSize("pp".ToSnakeCase().ToLower(), 1);  // Extra Small
        public static readonly FashionSize P = new FashionSize("p".ToSnakeCase().ToLower(), 2);   // Small
        public static readonly FashionSize M = new FashionSize("m".ToSnakeCase().ToLower(), 3);   // Medium
        public static readonly FashionSize G = new FashionSize("g".ToSnakeCase().ToLower(), 4);   // Large
        public static readonly FashionSize GG = new FashionSize("gg".ToSnakeCase().ToLower(), 5); // Extra Large
        public static readonly FashionSize XG = new FashionSize("xg".ToSnakeCase().ToLower(), 6); // Extra Extra Large
        public static readonly FashionSize XXG = new FashionSize("xxg".ToSnakeCase().ToLower(), 6); // Extra Extra Large
        public static readonly FashionSize XXXG = new FashionSize("xxxg".ToSnakeCase().ToLower(), 6); // Extra Extra Large
        public static readonly FashionSize PlusSize = new FashionSize("PlusSize".ToSnakeCase().ToLower(), 6); // Extra Extra Large

        private FashionSize(string name, int value) : base(name, value) { }
    }
}
