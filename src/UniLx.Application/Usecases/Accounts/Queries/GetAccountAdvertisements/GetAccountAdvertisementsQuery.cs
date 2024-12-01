using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Queries.GetAccountAdvertisements
{
    public class GetAccountAdvertisementsQuery : IQuery<IResult>
    {
        public string AccountId { get; private set; }
        public bool SortAsc { get; private set; }
        public string? Type { get; private set; }
        public string? CategoryName { get; private set; }
        public string? Status { get; private set; }
        public bool IncludeExpired { get; private set; }
        public DateTime? CreatedSince { get; private set; }
        public DateTime? CreatedUntil { get; private set; }

        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public GetAccountAdvertisementsQuery(string accountId, string? type, string? categoryName, 
            bool? sortAsc, string? status, bool? includeExpired, DateTime? createdSince, 
            DateTime? createdUntil, int? page, int? pageSize)
        {
            AccountId = accountId;
            Type = type;
            CategoryName = categoryName;
            Status = status;
            IncludeExpired = includeExpired ?? false;
            CreatedSince = createdSince;
            CreatedUntil = createdUntil;
            Page = page ?? 1;
            PageSize = pageSize ?? 30;
            SortAsc = sortAsc ?? false;
        }
    }

    public class GetAccountAdvertisementsQueryValidator : AbstractValidator<GetAccountAdvertisementsQuery>
    {
        public GetAccountAdvertisementsQueryValidator()
        {
            RuleFor(query => query.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");

            RuleFor(query => query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(30).WithMessage("PageSize must not exceed 30.");

            RuleFor(query => query.Type)
                .MaximumLength(50).WithMessage("Type must not exceed 50 characters.")
                .When(query => query.Type != null)
                .Must(type => AdvertisementType.TryFromName(type, true, out _))
                .WithMessage($"Invalid advertisement type. Supported types are: {string.Join(", ", AdvertisementType.List)}")
                .When(query => query.Type != null);

            RuleFor(query => query.CategoryName)
                .MaximumLength(50).WithMessage("CategoryName must not exceed 50 characters.")
                .When(query => query.CategoryName != null);

            RuleFor(query => query)
                .Must(query => !(query.CategoryName != null && query.Type != null))
                .WithMessage("Only one of CategoryName or Type can be provided, not both.")
                .WithName("CategoryName and Type");

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
