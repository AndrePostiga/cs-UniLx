using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountByCognitoSub
{
    public sealed class GetAccountByCognitoSubQueryExternal(string subId) : IQuery<IResult>
    {
        public string SubId { get; private set; } = subId;
    }
}
