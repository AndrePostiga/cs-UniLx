using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Mappers;
using UniLx.Domain.Data;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisementById
{
    internal class GetAdvertisementByIdQueryHandler : IQueryHandler<GetAdvertisementByIdQuery, IResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IStorageRepository<AccountAvatarBucketOptions> _accountStorage;

        public GetAdvertisementByIdQueryHandler(IAdvertisementRepository advertisementRepository,
            IAccountRepository accountRepository,
            IStorageRepository<AccountAvatarBucketOptions> accountStorage)
        {
            _advertisementRepository = advertisementRepository;
            _accountRepository = accountRepository;
            _accountStorage = accountStorage;
        }

        public async Task<IResult> Handle(GetAdvertisementByIdQuery request, CancellationToken cancellationToken)
        {
            var advertisement = await _advertisementRepository.FindOne(x => x.Id == request.Id, cancellationToken);
            if (advertisement == null)
                return AdvertisementErrors.NotFound.ToBadRequest();

            var account = await _accountRepository.FindOne(x => x.Id == advertisement.OwnerId, cancellationToken);
            if (account == null)
                return AdvertisementErrors.AccountNotFound.ToBadRequest();

            var imageUrl = account.ProfilePicture is null ? string.Empty : await _accountStorage.GetImageUrl(account.ProfilePicture);
            return Results.Ok(advertisement.ToResponse(account, imageUrl));
        }
    }
}
