using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public sealed class AgeRestriction : SmartEnum<AgeRestriction>
    {
        public static readonly AgeRestriction Free = new AgeRestriction(nameof(Free).ToLower(), 1);
        public static readonly AgeRestriction Age10 = new AgeRestriction(nameof(Age10).ToLower(), 2);
        public static readonly AgeRestriction Age12 = new AgeRestriction(nameof(Age12).ToLower(), 3);
        public static readonly AgeRestriction Age14 = new AgeRestriction(nameof(Age14).ToLower(), 4);
        public static readonly AgeRestriction Age16 = new AgeRestriction(nameof(Age16).ToLower(), 5);
        public static readonly AgeRestriction Age18 = new AgeRestriction(nameof(Age18).ToLower(), 6);


        public AgeRestriction(string name, int value) : base(name, value)
        {
        }
    }
}
