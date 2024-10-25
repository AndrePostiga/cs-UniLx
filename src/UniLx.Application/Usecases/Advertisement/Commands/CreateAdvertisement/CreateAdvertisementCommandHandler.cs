using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts;
using UniLx.Application.Usecases.Categories;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Infra.Data.Database;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement
{
    internal class CreateAdvertisementCommandHandler : ICommandHandler<CreateAdvertisementCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IMartenContext _martenContext;

        public CreateAdvertisementCommandHandler(
            IMartenContext martenContext,
            IAccountRepository accountRepository, 
            ICategoryRepository categoryRepository, 
            IAdvertisementRepository advertisementRepository)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _advertisementRepository = advertisementRepository;
            _martenContext = martenContext;
        }

        public async Task<IResult> Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.AccountId, cancellationToken);
            if (account == null)
                return AccountErrors.NotFound.ToBadRequest();

            var category = await _categoryRepository.FindOne(x => x.Name == request.SubCategory, cancellationToken);
            if (category == null)
                return CategoryErrors.NotFound.ToBadRequest();

            var details = new BeautyDetails(
                request.BeautyDetails.Title,
                request.BeautyDetails.Description,
                request.BeautyDetails.Price,
                request.BeautyDetails.ProductType,
                request.BeautyDetails.Brand,
                request.BeautyDetails.SkinType,
                request.BeautyDetails.ExpirationDate,
                request.BeautyDetails.Ingredients,
                request.BeautyDetails.IsOrganic);


            var address = new Address(request.Latitude, request.Longitude);
            var advertisement = new Domain.Entities.AdvertisementAgg.Advertisement(
                request.Type,
                category,
                details,
                DateTime.UtcNow.AddDays(20),
                address,
                account);

            _advertisementRepository.InsertOne(advertisement);
            _advertisementRepository.CustomSql(
                CustomQueries.InsertGeopointOnAdvertisementTable,
                address.Longitude,
                address.Latitude,
                advertisement.Id
            );
            await _advertisementRepository.UnitOfWork.Commit();
            return Results.Ok(advertisement);
        }
    }
}
