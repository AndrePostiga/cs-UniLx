using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture.Models;
using UniLx.Domain.Data;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture
{
    internal class UpdateProfilePictureCommandHandler : ICommandHandler<UpdateProfilePictureCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStorageRepository<AccountAvatarBucketOptions> _storageRepository;

        public UpdateProfilePictureCommandHandler(IAccountRepository accountRepository, IStorageRepository<AccountAvatarBucketOptions> storageRepository)
        {
            _accountRepository = accountRepository;
            _storageRepository = storageRepository;
        }

        public async Task<IResult> Handle(UpdateProfilePictureCommand request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(acc => acc.Id == request.AccountId, cancellationToken);
            if (account is null)
                return AccountErrors.NotFound.ToBadRequest();

            account.UpdateProfilePicture(request.ProfilePicture!);

            var expiresAt = DateTime.UtcNow.AddMinutes(5);
            string? imageUrl = string.Empty;
            if (account.ProfilePicture is not null)
            {
                imageUrl = await _storageRepository.GetImageUrl(account.ProfilePicture, expiresAt);

                if (string.IsNullOrWhiteSpace(imageUrl))
                    return AccountErrors.ProfilePictureNotUploaded.ToBadRequest();
            }

            _accountRepository.UpdateOne(account);
            await _accountRepository.UnitOfWork.Commit(cancellationToken);
            return Results.Ok(new UpdateProfilePictureResponse(imageUrl, expiresAt));
        }
    }
}
