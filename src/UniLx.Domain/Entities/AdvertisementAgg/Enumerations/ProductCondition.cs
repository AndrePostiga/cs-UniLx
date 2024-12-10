using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public sealed class ProductCondition : SmartEnum<ProductCondition>
    {

        public static readonly ProductCondition New = new ProductCondition(nameof(New).ToSnakeCase().ToLower(), 1);
        public static readonly ProductCondition LikeNew = new ProductCondition(nameof(LikeNew).ToSnakeCase().ToLower(), 2);
        public static readonly ProductCondition Used = new ProductCondition(nameof(Used).ToSnakeCase().ToLower(), 3);
        public static readonly ProductCondition Refurbished = new ProductCondition(nameof(Refurbished).ToSnakeCase().ToLower(), 4);


        public ProductCondition(string name, int value) : base(name, value)
        {
        }
    }
}
