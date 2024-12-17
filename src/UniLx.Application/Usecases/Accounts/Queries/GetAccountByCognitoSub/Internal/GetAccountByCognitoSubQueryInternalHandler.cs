using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountByCognitoSub.Internal
{
    internal class GetAccountByCognitoSubQueryInternalHandler(IAccountRepository accountRepository)
        : IQueryInternalHandler<GetAccountByCognitoSubQueryInternal, Account?>
    {
        private readonly IAccountRepository _accountRepository = accountRepository;


        public async Task<Account?> Handle(GetAccountByCognitoSubQueryInternal request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.CognitoSubscriptionId == request.SubId, cancellationToken);
            if (account == null)
                return null;

            return account;
        }
    }
}
