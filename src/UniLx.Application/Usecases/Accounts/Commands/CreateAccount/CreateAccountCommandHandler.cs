using Microsoft.AspNetCore.Http;
using UniLx.ApiService.Abstractions;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AccountAgg.ValueObj;

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
            var account = new Account("Andre", new Email("andre.postiga@msn.com"), new CPF("15792539707"), "stub path");

            _accountRepository.InsertOne(account);

            await _accountRepository.UnitOfWork.Commit(cancellationToken);

            return Results.Ok(account);
        }
    }
}
