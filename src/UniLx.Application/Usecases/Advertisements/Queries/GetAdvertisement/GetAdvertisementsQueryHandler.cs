using MediatR;
using Microsoft.AspNetCore.Http;
using System.Threading;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Mappers;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Models;
using UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Spec;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement
{
    internal class GetAdvertisementsQueryHandler : IQueryHandler<GetAdvertisementsQuery, IResult>
    {
        private readonly IAdvertisementRepository _advertisementRepository;

        public GetAdvertisementsQueryHandler(IAdvertisementRepository advertisementRepository)
        {
            _advertisementRepository = advertisementRepository;
        }

        public async Task<IResult> Handle(GetAdvertisementsQuery request, CancellationToken cancellationToken)
        {
            var (advertisements, count) = await HandleLocationAdvertisementsSearch(request, cancellationToken);
            var response = advertisements!.Select(ad => ad.ToResponse());
            var result = PaginatedQueryResponse<GetAdvertisementsResponse>.WithContent(response, request.Page, request.PageSize, count);
            return Results.Ok(result);
        }

        private async Task<Tuple<IEnumerable<Advertisement>?, int>> HandleLocationAdvertisementsSearch(GetAdvertisementsQuery request, CancellationToken ct)
            => await _advertisementRepository.FindAdvertisements(skip: request.Page,
                    limit: request.PageSize,
                    sortAsc: true,
                    request.ToSpec(),
                    request.Geopoint!,
                    request.RadiusInKm,
                    ct);
    }
}
