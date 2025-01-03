using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.SharedModels.Mappers;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Commands.RateAdvertisement
{
    internal class RateAdvertisementCommandHandler : ICommandHandler<RateAdvertisementCommand, IResult>
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IAccountRepository _accountRepository;

        public RateAdvertisementCommandHandler(IAdvertisementRepository advertisementRepository,
            IAccountRepository accountRepository)
        {
            _advertisementRepository = advertisementRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IResult> Handle(RateAdvertisementCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.AccountId, cancellationToken);
            if (account == null)
                return AdvertisementErrors.AccountNotFound.ToBadRequest();

            Account? ownerAccount = null;
            var advertisement = await _advertisementRepository.FindOneWithInclude<Account>(
                x => x.Id == request.AdvertisementId,
                x => x.OwnerId,
                x => ownerAccount = x,
                cancellationToken);

            if (advertisement == null || ownerAccount == null)
                return AdvertisementErrors.NotFound.ToBadRequest();

            if (advertisement.Status.Name != AdvertisementStatus.Finished.Name)
                return AdvertisementErrors.InvalidStatusForRating.ToBadRequest();

            advertisement.Rate(request.Rating, account, ownerAccount);
            
            _advertisementRepository.UpdateOne(advertisement);
            _accountRepository.UpdateOne(ownerAccount);
            await _advertisementRepository.UnitOfWork.Commit(cancellationToken);            
            return Results.Ok(advertisement.Rating.ToResponse());
        }
    }
}
