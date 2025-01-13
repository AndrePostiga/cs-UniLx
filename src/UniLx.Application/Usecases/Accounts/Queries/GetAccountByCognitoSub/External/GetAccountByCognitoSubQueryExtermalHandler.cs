using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Commands.CreateAccount.Mappers;
using UniLx.Domain.Data;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountByCognitoSub.External
{
    internal class GetAccountByCognitoSubQueryExtermalHandler(IAccountRepository accountRepository) 
        : IQueryHandler<GetAccountByCognitoSubQueryExternal, IResult>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;

        public async Task<IResult> Handle(GetAccountByCognitoSubQueryExternal request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.CognitoSubscriptionId == request.SubId, cancellationToken);
            if (account == null)
                return AccountErrors.NotFound.ToBadRequest();

            return Results.Ok(account.ToResponse());
        }
    }
}
