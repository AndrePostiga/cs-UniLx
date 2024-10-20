﻿using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.CreateAccount
{
    internal class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IStorageRepository<AccountAvatarBucketOptions> _storageRepository;

        public CreateAccountCommandHandler(IAccountRepository accountRepository, IStorageRepository<AccountAvatarBucketOptions> storageRepository)
        {
            _accountRepository = accountRepository;
            _storageRepository = storageRepository;
        }

        public async Task<IResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {

            Expression<Func<Account, bool>> spec =
                acc => acc.Cpf.Value == request.Cpf ||
                acc.Email.Value == request.Email ||
                acc.ProfilePicture!.Path == request.ProfilePicturePath;

            var account = await _accountRepository.FindOne(spec, cancellationToken);
            if (account is not null)
                return AccountErrors.Conflict.ToBadRequest();

            account = new Account(request.Name, request.Email, request.Cpf, request.Description, request.ProfilePicturePath);

            string imageUrl = string.Empty;
            if (account.ProfilePicture is not null)
            {
                imageUrl = await _storageRepository.GetImageUrl(account.ProfilePicture);

                if (string.IsNullOrWhiteSpace(imageUrl))
                    return AccountErrors.ProfilePictureNotUploaded.ToBadRequest();
            }

            _accountRepository.InsertOne(account);
            await _accountRepository.UnitOfWork.Commit(cancellationToken);
            
            return Results.Ok(account.ToResponse(imageUrl));
        }
    }
}
