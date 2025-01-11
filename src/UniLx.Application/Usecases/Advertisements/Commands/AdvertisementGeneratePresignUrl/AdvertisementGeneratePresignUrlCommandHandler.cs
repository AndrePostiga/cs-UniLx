using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Advertisements.Commands.AdvertisementGeneratePresignUrl.Models;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Commands.AdvertisementGeneratePresignUrl
{
    internal class AdvertisementGeneratePresignUrlCommandHandler : ICommandHandler<AdvertisementGeneratePresignUrlCommand, IResult>
    {
        private readonly IStorageRepository<AdvertisementBucketOptions> _storageRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IAdvertisementRepository _advertisementRepository;

        public AdvertisementGeneratePresignUrlCommandHandler(IStorageRepository<AdvertisementBucketOptions> storageRepository,
            IAccountRepository accountRepository,
            IAdvertisementRepository advertisementRepository)
        {
            _storageRepository = storageRepository;
            _accountRepository = accountRepository;
            _advertisementRepository = advertisementRepository;
        }

        public async Task<IResult> Handle(AdvertisementGeneratePresignUrlCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.AccountId, cancellationToken);
            if (account == null)
                return AdvertisementErrors.AccountNotFound.ToBadRequest();

            var advertisement = await _advertisementRepository.FindOne(x => x.Id == request.AdvertisementId, cancellationToken);
            if (advertisement == null)
                return AdvertisementErrors.NotFound.ToBadRequest();
            
            var image = Image.Create(request.FileName);
            var filePath = $"{request.AdvertisementId}/{request.FileName}";
            var expiresAt = DateTime.UtcNow.AddMinutes(15);

            var signedUrl = await _storageRepository.GeneratePreSignedUrlAsync(
                path: $"{request.AdvertisementId}/{image.FileName}",
                expiresAt: DateTime.UtcNow.AddMinutes(30));

            if (string.IsNullOrWhiteSpace(signedUrl))
            {
                return AdvertisementErrors.ErrorGeneratingPresignUrl.ToBadRequest();
            }

            return Results.Ok(new AdvertisementPresignUrlResponse(signedUrl, image.FileName, expiresAt));
        }
    }
}
