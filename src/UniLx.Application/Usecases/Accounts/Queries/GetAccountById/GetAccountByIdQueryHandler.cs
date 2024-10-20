using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Domain.Data;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountById
{
    internal class GetAccountByIdQueryHandler(
        IAccountRepository accountRepository, 
        IStorageRepository<AccountAvatarBucketOptions> accountStorage) : IQueryHandler<GetAccountByIdQuery, IResult>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;
        private readonly IStorageRepository<AccountAvatarBucketOptions> _accountStorage = accountStorage;

        public async Task<IResult> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.Id, cancellationToken);
            if (account == null)
                return AccountErrors.NotFound.ToBadRequest();

            string? imageUrl = null;
            if (account.ProfilePicture is not null)
                imageUrl = await _accountStorage.GetImageUrl(account.ProfilePicture);

            return Results.Ok(account.ToResponse(imageUrl));
        }
    }
}
