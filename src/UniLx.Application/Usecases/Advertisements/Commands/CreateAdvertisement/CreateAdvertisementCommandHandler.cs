using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Factories;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers;
using UniLx.Application.Usecases.Categories;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Services;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;
using UniLx.Shared.LibExtensions;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement
{
    public class CreateAdvertisementCommandHandler : ICommandHandler<CreateAdvertisementCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ICreateAdvertisementDomainService _createAdvertisementDomainService;
        private readonly IStorageRepository<AccountBucketOptions> _accountStorage;      

        public CreateAdvertisementCommandHandler(
            IAccountRepository accountRepository,
            IStorageRepository<AccountBucketOptions> accountStorage,
            ICategoryRepository categoryRepository, 
            ICreateAdvertisementDomainService createAdvertisementDomainService)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _createAdvertisementDomainService = createAdvertisementDomainService;
            _accountStorage = accountStorage;
        }

        public async Task<IResult> Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.AccountId, cancellationToken);
            if (account == null)
                return AdvertisementErrors.AccountNotFound.ToBadRequest();

            var advertisementType = AdvertisementType.FromName(request.Type);

            var category = await _categoryRepository.FindOne(x => x.Name == request.SubCategory &&
                x.Root.HasSmartEnumValue(advertisementType) &&
                x.IsDeleted == false &&
                x.IsActive == true, cancellationToken);

            if (category == null)
                return CategoryErrors.NotFound.ToBadRequest();

            var advertisement = await _createAdvertisementDomainService.CreateAdvertisement(AdvertisementType.FromName(request.Type, true),
                category,
                request.ToDetails(),
                request.ExpiresAt,
                account,
                request.Address!.ToAddress(),
                cancellationToken);

            if (advertisement.IsError)
                return advertisement.Error.ToBadRequest();
            
            var result = advertisement.Content!.ToResponse(account);
            return Results.Ok(result);
        }
    }
}
