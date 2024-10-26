﻿using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommand;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request.DetailsRequest;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers
{
    public static class CreateAdvertisementRequestToCommandMapper
    {
        public static CreateAdvertisementCommand ToCommand(this CreateAdvertisementRequest source, string accountId)
            => new (
                accountId: accountId,
                type: source.Type,
                subCategory: source.SubCategory,
                expiresAt: source.ExpiresAt,
                address: source.Address?.ToCommand(),
                beautyDetails: source.BeautyDetails?.ToCommand());

        public static CreateAddressCommand ToCommand(this AddressRequest source)
            => new (
                latitude: source.Latitude,
                longitude: source.Longitude,
                country: source.Country,
                state: source.State,
                city: source.City,
                neighborhood: source.Neighborhood,
                zipCode: source.ZipCode,
                street: source.Street,
                number: source.Number,
                complement: source.Complement
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
