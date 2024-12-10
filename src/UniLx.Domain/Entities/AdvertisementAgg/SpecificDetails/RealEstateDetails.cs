using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class RealEstateDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.RealEstate;

        public double LotSizeInSquareMeters { get; private set; }
        public double? ConstructedSquareFootage { get; private set; }
        public PropertyType PropertyType { get; private set; }
        public PropertyCondition? Condition { get; private set; }
        public int? Bedrooms { get; private set; }
        public int? Bathrooms { get; private set; }
        public int? ParkingSpaces { get; private set; }
        public int? Floors { get; private set; }
        public List<string>? AdditionalFeatures { get; private set; }

        private RealEstateDetails() : base() { }

        public RealEstateDetails(
            string? title,
            string? description,
            int? price,
            double lotSizeInSquareMeters,
            string? propertyType,
            string? condition,
            double? constructedSquareFootage = null,
            int? bedrooms = null,
            int? bathrooms = null,
            int? parkingSpaces = null,
            int? floors = null,
            List<string>? additionalFeatures = null
        ) : base(title, description, price)
        {
            SetLotSizeInSquareMeters(lotSizeInSquareMeters);
            SetPropertyType(propertyType);
            SetCondition(condition);
            SetConstructedSquareFootage(constructedSquareFootage);
            SetBedrooms(bedrooms);
            SetBathrooms(bathrooms);
            SetParkingSpaces(parkingSpaces);
            SetFloors(floors);
            SetAdditionalFeatures(additionalFeatures);
        }

        protected override void SetPrice(int? price)
        {
            DomainException.ThrowIf(!price.HasValue, "Price is required for RealEstate details.");
            DomainException.ThrowIf(price!.Value <= 0, $"{nameof(Price)} cannot be less or equal than 0");
            DomainException.ThrowIf(price.Value > 100_000_000, $"{nameof(Price)} cannot be more than R$ 1.000.000,00");
            Price = price.Value;
        }

        private void SetLotSizeInSquareMeters(double lotSizeInSquareMeters)
        {
            DomainException.ThrowIf(lotSizeInSquareMeters <= 0, "Lot size must be greater than zero.");
            LotSizeInSquareMeters = lotSizeInSquareMeters;
        }

        private void SetPropertyType(string? propertyTypeName)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(propertyTypeName), "PropertyType cannot be null or empty.");
            var isValidPropertyType = PropertyType.TryFromName(propertyTypeName, ignoreCase: true, out var parsedPropertyType);
            DomainException.ThrowIf(!isValidPropertyType, $"Invalid PropertyType. Allowed values are: {string.Join(", ", PropertyType.List.Select(p => p.Name))}.");
            PropertyType = parsedPropertyType!;
        }

        private void SetCondition(string? conditionName)
        {
            if (PropertyType.Name == PropertyType.Land.Name)
            {
                DomainException.ThrowIf(!string.IsNullOrWhiteSpace(conditionName), "Condition must not have a value for land.");
                Condition = null;
                return;
            }

            DomainException.ThrowIf(string.IsNullOrWhiteSpace(conditionName), "Condition is required for non-land properties.");
            var hasCondition = PropertyCondition.TryFromName(conditionName, ignoreCase: true, out var cond);
            DomainException.ThrowIf(!hasCondition, $"Invalid condition. Allowed values are: {string.Join(", ", PropertyCondition.List.Select(c => c.Name))}.");
            Condition = cond!;
        }

        private void SetConstructedSquareFootage(double? constructedSquareFootage)
        {
            if (constructedSquareFootage.HasValue)
            {
                DomainException.ThrowIf(constructedSquareFootage.Value <= 0, "Constructed square footage, if provided, must be greater than zero.");
            }
            ConstructedSquareFootage = constructedSquareFootage;
        }

        private void SetBedrooms(int? bedrooms)
        {
            if (PropertyType.Name == PropertyType.Land.Name)
            {
                DomainException.ThrowIf(bedrooms.HasValue, "Bedrooms must not have a value for land.");
                return;
            }

            if (bedrooms.HasValue)
            {
                DomainException.ThrowIf(bedrooms.Value < 0, "Bedrooms cannot be negative.");
                DomainException.ThrowIf(bedrooms.Value > 100, "Bedrooms number seems too high.");
            }
            Bedrooms = bedrooms;
        }

        private void SetBathrooms(int? bathrooms)
        {
            if (PropertyType.Name == PropertyType.Land.Name)
            {
                DomainException.ThrowIf(bathrooms.HasValue, "Bathrooms must not have a value for land.");
                return;
            }

            if (bathrooms.HasValue)
            {
                DomainException.ThrowIf(bathrooms.Value < 0, "Bathrooms cannot be negative.");
                DomainException.ThrowIf(bathrooms.Value > 50, "Bathrooms number seems too high.");
            }
            Bathrooms = bathrooms;
        }

        private void SetParkingSpaces(int? parkingSpaces)
        {
            if (PropertyType.Name == PropertyType.Land.Name)
            {
                DomainException.ThrowIf(parkingSpaces.HasValue, "ParkingSpaces must not have a value for land.");
                return;
            }

            if (parkingSpaces.HasValue)
            {
                DomainException.ThrowIf(parkingSpaces.Value < 0, "Parking spaces cannot be negative.");
                DomainException.ThrowIf(parkingSpaces.Value > 100, "Parking spaces number seems too high.");
            }
            ParkingSpaces = parkingSpaces;
        }

        private void SetFloors(int? floors)
        {
            if (PropertyType.Name == PropertyType.Land.Name)
            {
                DomainException.ThrowIf(floors.HasValue, "Floors must not have a value for land.");
                return;
            }

            if (floors.HasValue)
            {
                DomainException.ThrowIf(floors.Value < 0, "Floors cannot be negative.");
                DomainException.ThrowIf(floors.Value > 100, "Floors number seems too high.");
            }
            Floors = floors;
        }

        private void SetAdditionalFeatures(List<string>? additionalFeatures)
        {
            if (additionalFeatures != null && additionalFeatures.Count > 0)
            {
                DomainException.ThrowIf(additionalFeatures.Any(a => string.IsNullOrWhiteSpace(a)), "AdditionalFeatures cannot contain null or empty values.");
                DomainException.ThrowIf(additionalFeatures.Any(a => a.Length > 50), "Each additional feature must be 50 characters or less.");
            }
            AdditionalFeatures = additionalFeatures;
        }
    }
}
