using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Mappers;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Commands.FinishAdvertisement
{
    internal class FinishAdvertisementCommandHandler : ICommandHandler<FinishAdvertisementCommand, IResult>
    {
        private readonly IAdvertisementRepository _advertisementRepository;

        public FinishAdvertisementCommandHandler(IAdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }

        public async Task<IResult> Handle(FinishAdvertisementCommand request, CancellationToken cancellationToken)
        {
            Account? ownerAccount = null;
            var advertisement = await _advertisementRepository.FindOneWithInclude<Account>(
                x => x.Id == request.AdvertisementId,
                x => x.OwnerId,
                x => ownerAccount = x,
                cancellationToken);

            if (advertisement == null || ownerAccount == null)
                return AdvertisementErrors.NotFound.ToBadRequest();

            advertisement.Finish(ownerAccount);
            _advertisementRepository.UpdateOne(advertisement);
            await _advertisementRepository.UnitOfWork.Commit(cancellationToken);
            return Results.Ok(advertisement.ToResponse());
        }
    }
}
