using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountInterestedAdvertisements
{
    public class GetAccountInterestedAdvertisementsQuery : IQuery<IResult>
    {
        public string AccountId { get; private set; }
        public bool SortAsc { get; private set; }
        public string? Status { get; private set; }
        public bool IncludeExpired { get; private set; }
        public DateTime? CreatedSince { get; private set; }
        public DateTime? CreatedUntil { get; private set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public GetAccountInterestedAdvertisementsQuery(string accountId, bool? sortAsc, string? status, bool? includeExpired, DateTime? createdSince, DateTime? createdUntil, int? page, int? pageSize)
        {
            AccountId = accountId;
            SortAsc = sortAsc ?? false;
            Status = status;
            IncludeExpired = includeExpired ?? false;
            CreatedSince = createdSince;
            CreatedUntil = createdUntil;
            Page = page ?? 1;
            PageSize = pageSize ?? 30;
        }
    }

    public class GetAccountInterestedAdvertisementsQueryValidator : AbstractValidator<GetAccountInterestedAdvertisementsQuery>
    {
        public GetAccountInterestedAdvertisementsQueryValidator()
        {
            RuleFor(query => query.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");

            RuleFor(query => query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(30).WithMessage("PageSize must not exceed 30.");

            RuleFor(query => query.CreatedSince)
                .LessThanOrEqualTo(DateTime.Now).WithMessage("CreatedSince must be in the past.")
                .When(query => query.CreatedSince.HasValue);

            RuleFor(query => query.CreatedUntil)
                .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage("CreatedUntil must only be +1 day from today.")
                .When(query => query.CreatedUntil.HasValue);

            RuleFor(query => query)
                .Must(query => query.CreatedSince == null || query.CreatedUntil == null || query.CreatedSince <= query.CreatedUntil)
                .WithMessage("CreatedSince must be less than or equal to CreatedUntil.")
                .WithName("CreatedSince and CreatedUntil");
        }
    }
}
