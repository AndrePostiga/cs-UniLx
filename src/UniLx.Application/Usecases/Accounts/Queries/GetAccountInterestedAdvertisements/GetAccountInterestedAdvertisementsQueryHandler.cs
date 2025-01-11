using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Mappers;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements.Models;
using UniLx.Application.Usecases.Accounts.Queries.GetAccountInterestedAdvertisements.Spec;
using UniLx.Application.Usecases.Advertisements;
using UniLx.Domain.Data;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountInterestedAdvertisements
{
    public class GetAccountInterestedAdvertisementsQueryHandler : IQueryHandler<GetAccountInterestedAdvertisementsQuery, IResult>
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IAccountRepository _accountRepository;

        public GetAccountInterestedAdvertisementsQueryHandler(
            IAdvertisementRepository advertisementRepository,
            IAccountRepository accountRepository)
        {
            _advertisementRepository = advertisementRepository;
            _accountRepository = accountRepository;
        }

        public async Task<IResult> Handle(GetAccountInterestedAdvertisementsQuery request, CancellationToken cancellationToken)
        {
            var account = await _accountRepository.FindOne(x => x.Id == request.AccountId, cancellationToken);
            if (account == null)
                return AdvertisementErrors.AccountNotFound.ToBadRequest();

            var (advertisements, count) = await _advertisementRepository.FindAll(
                skip: request.Page,
                limit: request.PageSize,
                sortAsc: request.SortAsc,
                expression: request.ToSpec(account.InterestAdvertisementIds),
                cancellationToken);

            var response = advertisements?.Select(ad => ad.ToResponse());
            var result = PaginatedQueryResponse<GetAccountAdvertisementsResponse>.WithContent(response, request.Page, request.PageSize, count);
            return Results.Ok(result);
        }
    }
}
