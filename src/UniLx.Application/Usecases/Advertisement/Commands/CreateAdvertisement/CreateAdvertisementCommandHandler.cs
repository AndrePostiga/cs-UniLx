using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts;
using UniLx.Application.Usecases.Categories;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement
{
    internal class CreateAdvertisementCommandHandler : ICommandHandler<CreateAdvertisementCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAdvertisementRepository _advertisementRepository;

        public CreateAdvertisementCommandHandler(IAccountRepository accountRepository, 
            ICategoryRepository categoryRepository, 
            IAdvertisementRepository advertisementRepository)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _advertisementRepository = advertisementRepository;
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

            var advertisement = new Domain.Entities.AdvertisementAgg.Advertisement(
                request.Type,
                category,
                details,
                null,
                null,
                account,
                request.Latitude,
                request.Longitude);

            _advertisementRepository.InsertOne(advertisement);
            await _advertisementRepository.UnitOfWork.Commit();
            return Results.Ok(advertisement);
        }
    }
}
