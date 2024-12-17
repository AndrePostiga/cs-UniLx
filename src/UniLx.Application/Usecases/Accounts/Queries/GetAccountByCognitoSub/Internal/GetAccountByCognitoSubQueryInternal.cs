using Microsoft.AspNetCore.Http;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountByCognitoSub.Internal
{
    public sealed class GetAccountByCognitoSubQueryInternal(string subId) : IQueryInternal<Account?>
    {
        public string SubId { get; private set; } = subId;
    }
}
