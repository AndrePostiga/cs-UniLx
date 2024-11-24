using Ardalis.SmartEnum;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class FashionSize : SmartEnum<FashionSize>
    {
        public static readonly FashionSize PP = new FashionSize("PP".ToLower(), 1);  // Extra Small
        public static readonly FashionSize P = new FashionSize("P".ToLower(), 2);   // Small
        public static readonly FashionSize M = new FashionSize("M".ToLower(), 3);   // Medium
        public static readonly FashionSize G = new FashionSize("G".ToLower(), 4);   // Large
        public static readonly FashionSize GG = new FashionSize("GG".ToLower(), 5); // Extra Large
        public static readonly FashionSize XG = new FashionSize("XG".ToLower(), 6); // Extra Extra Large
        public static readonly FashionSize XXG = new FashionSize("XXG".ToLower(), 6); // Extra Extra Large
        public static readonly FashionSize XXXG = new FashionSize("XXXG".ToLower(), 6); // Extra Extra Large
        public static readonly FashionSize PlusSize = new FashionSize("PlusSize".ToLower(), 6); // Extra Extra Large

        private FashionSize(string name, int value) : base(name, value) { }
    }
}
