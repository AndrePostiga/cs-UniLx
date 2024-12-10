using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.CreateAccount
{
    internal class CreateAccountCommandHandler : ICommandHandler<CreateAccountCommand, IResult>
    {
        private readonly IAccountRepository _accountRepository;

        public CreateAccountCommandHandler(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<IResult> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            Expression<Func<Account, bool>> spec =
                acc => acc.Cpf.Value == request.Cpf ||
                acc.Email.Value == request.Email;

            var account = await _accountRepository.FindOne(spec, cancellationToken);
            if (account is not null)
                return AccountErrors.AccountConflict.ToBadRequest();

            account = new Account(request.Name, request.Email, request.Cpf, request.Description);

            _accountRepository.InsertOne(account);
            await _accountRepository.UnitOfWork.Commit(cancellationToken);
            
            return Results.Ok(account.ToResponse());
        }
    }
}
