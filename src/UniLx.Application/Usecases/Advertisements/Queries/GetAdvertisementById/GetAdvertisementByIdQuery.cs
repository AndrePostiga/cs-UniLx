using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisementById
{
    public sealed class GetAdvertisementByIdQuery(string id) : IQuery<IResult>
    {
        public string Id { get; private set; } = id;
    }
}
