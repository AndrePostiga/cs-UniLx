using FluentValidation;
using Microsoft.AspNetCore.Http;
using NetTopologySuite.Geometries;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement
{
    public class GetAdvertisementsQuery : IQuery<IResult>
    {
        public string? Type { get; set; }
        public string? CategoryName { get; set; }

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? RadiusInKm { get; set; }

        // Pagination
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public Geometry? Geopoint => HasGeolocation
            ? new Point(Longitude!.Value, Latitude!.Value) { SRID = 4326 }
            : null;

        public bool HasGeolocation => Latitude.HasValue && Longitude.HasValue;

        public GetAdvertisementsQuery(string? type, string? categoryName,
            double? latitude, double? longitude, double? radiusInKm, int page, int pageSize)
        {
            Type = type;
            CategoryName = categoryName;
            Latitude = latitude;
            Longitude = longitude;
            RadiusInKm = radiusInKm;
            Page = page;
            PageSize = pageSize;
        }
    }

    public class GetAdvertisementsQueryValidator : AbstractValidator<GetAdvertisementsQuery>
    {
        public GetAdvertisementsQueryValidator()
        {
            RuleFor(query => query.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");

            RuleFor(query => query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("PageSize must not exceed 20.");

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

            RuleFor(query => query.Latitude)
                .InclusiveBetween(-90, 90).WithMessage("Latitude must be a valid number between -90 and 90.")
                .When(query => query.Latitude.HasValue);

            RuleFor(query => query.Longitude)
                .InclusiveBetween(-180, 180).WithMessage("Longitude must be a valid number between -180 and 180.")
                .When(query => query.Longitude.HasValue);

            RuleFor(query => query.RadiusInKm)
                .GreaterThan(0).WithMessage("RadiusInKm must be a valid positive number.")
                .When(query => query.RadiusInKm.HasValue);

            RuleFor(x => x)
               .Must(x => !(x.Latitude.HasValue ^ x.Longitude.HasValue))
               .WithMessage("Both latitude and longitude must be provided together if one is specified.");

            RuleFor(query => query)
                .Must(query => !query.RadiusInKm.HasValue || (query.Latitude.HasValue && query.Longitude.HasValue))
                .WithMessage("Latitude and Longitude must be provided if RadiusInKm is specified.");
        }
    }

}
