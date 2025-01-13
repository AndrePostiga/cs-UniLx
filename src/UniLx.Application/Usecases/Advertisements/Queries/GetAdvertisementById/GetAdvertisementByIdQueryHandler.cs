using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisementById.Mappers;
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
        private readonly IStorageRepository<AccountBucketOptions> _accountStorage;
        private readonly IStorageRepository<AdvertisementBucketOptions> _advertisementStorage;


        public GetAdvertisementByIdQueryHandler(IAdvertisementRepository advertisementRepository,
            IAccountRepository accountRepository,
            IStorageRepository<AccountBucketOptions> accountStorage,
            IStorageRepository<AdvertisementBucketOptions> advertisementStorage)
        {
            _advertisementRepository = advertisementRepository;
            _accountRepository = accountRepository;
            _accountStorage = accountStorage;
            _advertisementStorage = advertisementStorage;
        }

        public async Task<IResult> Handle(GetAdvertisementByIdQuery request, CancellationToken cancellationToken)
        {
            var advertisement = await _advertisementRepository.FindOne(x => x.Id == request.Id, cancellationToken);
            if (advertisement == null)
                return AdvertisementErrors.NotFound.ToBadRequest();

            var owner = await _accountRepository.FindOne(x => x.Id == advertisement.OwnerId, cancellationToken);
            if (owner == null)
                return AdvertisementErrors.AccountNotFound.ToBadRequest();            

            return Results.Ok(advertisement.ToResponse(owner));
        }
    }
}
