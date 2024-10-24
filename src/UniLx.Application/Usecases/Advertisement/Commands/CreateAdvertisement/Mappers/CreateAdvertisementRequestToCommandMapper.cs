using UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.DetailsCommand;
using UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.Models;

namespace UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.Mappers
{
    public static class CreateAdvertisementRequestToCommandMapper
    {
        public static CreateAdvertisementCommand ToCommand(this CreateAdvertisementRequest source, string accountId)
            => new (
                accountId,
                source.Type,
                source.SubCategory,
                source.ExpiresAt,
                source.Latitude,
                source.Longitude,
                null,
                source.BeautyDetails?.ToCommand()
            );

        public static CreateBeautyDetailsCommand ToCommand(this BeautyDetailsRequest source)
            => new (
                source.Title!,
                source.Description,
                source.Price,
                source.ProductType!,
                source.Brand!,
                source.SkinType!,
                source.ExpirationDate,
                source.Ingredients,
                source.IsOrganic ?? false
            );
    }
}
