using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Domain.Entities.Factories
{
    public interface DetailsFactoryStrategy
    {
        AdvertisementType Type { get; }
        Details CreateDetails(string title, string? description, int? price);
    }

    public static class DetailsFactory
    {
        // Strongly-typed method for BeautyDetails creation
        //public Details CreateBeautyDetails(string title, string? description, int? price, string productType, string brand, string skinType)
        //{
        //    if (_strategies.TryGetValue(AdvertisementType.Beauty, out var strategy) && strategy is BeautyDetailsFactoryStrategy beautyStrategy)
        //    {
        //        return beautyStrategy.CreateDetails(title, description, price, productType, brand, skinType);
        //    }

        //    throw new NotSupportedException($"Details for type {AdvertisementType.Beauty.Name} are not supported.");
        //}
    }
}
