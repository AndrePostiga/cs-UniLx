using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountById
{
    public sealed class GetAccountByIdQuery(string id) : IQuery<IResult>
    {
        public string Id { get; private set; } = id;
    }
}
