using Ardalis.SmartEnum;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public sealed class ProductCondition : SmartEnum<ProductCondition>
    {

        public static readonly ProductCondition New = new ProductCondition(nameof(New).ToLower(), 1);
        public static readonly ProductCondition LikeNew = new ProductCondition(nameof(LikeNew).ToLower(), 2);
        public static readonly ProductCondition Used = new ProductCondition(nameof(Used).ToLower(), 3);
        public static readonly ProductCondition Refurbished = new ProductCondition(nameof(Refurbished).ToLower(), 4);


        public ProductCondition(string name, int value) : base(name, value)
        {
        }
    }
}
