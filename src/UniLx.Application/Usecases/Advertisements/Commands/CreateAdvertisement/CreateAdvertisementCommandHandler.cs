using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Factories;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers;
using UniLx.Application.Usecases.Categories;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement
{
    internal class CreateAdvertisementCommandHandler : ICommandHandler<CreateAdvertisementCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IStorageRepository<AccountAvatarBucketOptions> _storageRepository;

        public CreateAdvertisementCommandHandler(
            IAccountRepository accountRepository,
            IStorageRepository<AccountAvatarBucketOptions> storageRepository,
            ICategoryRepository categoryRepository, 
            IAdvertisementRepository advertisementRepository)
        {
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
            _advertisementRepository = advertisementRepository;
            _storageRepository = storageRepository;
        }

        public async Task<IResult> Handle(CreateAdvertisementCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.AccountId, cancellationToken);
            if (account == null)
                return AdvertisementErrors.AccountNotFound.ToBadRequest();

            var category = await _categoryRepository.FindOne(x => x.Name == request.SubCategory, cancellationToken);
            if (category == null)
                return CategoryErrors.NotFound.ToBadRequest();

            var advertisement = new Advertisement(
                request.Type!,
                category,
                request.ToDetails(),
                request.ExpiresAt,
                request.Address!.ToAddress(),
                account);

            var imageUrl = await _storageRepository.GetImageUrl(account.ProfilePicture, DateTime.UtcNow.AddMinutes(30));

            _advertisementRepository.InsertOne(advertisement);
            _accountRepository.UpdateOne(account);
            await _advertisementRepository.UnitOfWork.Commit(cancellationToken);
            var result = advertisement.ToResponse(account, imageUrl);
            return Results.Ok(result);
        }
    }
}
