using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture.Models;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture
{
    internal class UpdateProfilePictureCommandHandler : ICommandHandler<UpdateProfilePictureCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStorageRepository<AccountBucketOptions> _storageRepository;

        public UpdateProfilePictureCommandHandler(IAccountRepository accountRepository, IStorageRepository<AccountBucketOptions> storageRepository)
        {
            _accountRepository = accountRepository;
            _storageRepository = storageRepository;
        }

        public async Task<IResult> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(acc => acc.Id == request.AccountId, cancellationToken);
            if (account is null)
                return AccountErrors.NotFound.ToBadRequest();

            var image = Image.Create(request.FileName);
            var expiresAt = DateTime.UtcNow.AddMinutes(TimeSpan.FromMinutes(15).Minutes);

            var signedUrl = await _storageRepository.GeneratePreSignedUrlAsync(
                path: $"{request.AccountId}/{image.FileName}",
                expiresAt: DateTime.UtcNow.AddMinutes(30));

            if (string.IsNullOrWhiteSpace(signedUrl))
                return AccountErrors.ErrorGeneratingPresignUrl.ToBadRequest();

            return Results.Ok(new UpdateProfilePictureResponse(signedUrl, image.FileName, expiresAt));
        }
    }
}
