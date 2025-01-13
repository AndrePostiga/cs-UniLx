using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Domain.Data;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountById
{
    internal class GetAccountByIdQueryHandler(IAccountRepository accountRepository) 
        : IQueryHandler<GetAccountByIdQuery, IResult>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<IResult> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.Id, cancellationToken);
            if (account == null)
                return AccountErrors.NotFound.ToBadRequest();

            return Results.Ok(account.ToResponse());
        }
    }
}
