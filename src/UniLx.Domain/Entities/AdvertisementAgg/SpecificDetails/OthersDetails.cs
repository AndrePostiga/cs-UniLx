using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class OthersDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.Others;

        public ProductCondition? Condition { get; private set; }
        public string? Brand { get; private set; }
        public List<string>? Features { get; private set; }
        public DateTime? WarrantyUntil { get; private set; }

        private OthersDetails() : base() { }

        public OthersDetails(
            string? title,
            string? description,
            int? price,
            string? conditionName = null,
            string? brand = null,
            List<string>? features = null,
            DateTime? warrantyUntil = null
        ) : base(title, description, price)
        {
            SetCondition(conditionName);
            SetBrand(brand);
            SetFeatures(features);
            SetWarrantyUntil(warrantyUntil);
        }

        private void SetCondition(string? conditionName)
        {
            if (string.IsNullOrWhiteSpace(conditionName))
            {
                Condition = null;
                return;
            }

            var success = ProductCondition.TryFromName(conditionName, ignoreCase: true, out var productCondition);
            DomainException.ThrowIf(!success, $"Invalid product condition. Possible values are: {string.Join(", ", ProductCondition.List.Select(c => c.Name))}.");

            Condition = productCondition!;
        }

        private void SetBrand(string? brand)
        {
            if (!string.IsNullOrWhiteSpace(brand))
            {
                DomainException.ThrowIf(brand.Length > 100, "Brand must be 100 characters or less.");
            }
            Brand = brand;
        }

        private void SetFeatures(List<string>? features)
        {
            if (features != null && features.Count > 0)
            {
                DomainException.ThrowIf(features.Any(f => string.IsNullOrWhiteSpace(f)), "Features cannot contain null or empty values.");
                DomainException.ThrowIf(features.Any(f => f.Length > 50), "Each feature must be 50 characters or less.");
            }
            Features = features;
        }

        private void SetWarrantyUntil(DateTime? warrantyUntil)
        {
            if (warrantyUntil.HasValue)
            {
                DomainException.ThrowIf(warrantyUntil.Value <= DateTime.UtcNow, "WarrantyUntil must be in the future.");
            }
            WarrantyUntil = warrantyUntil;
        }
    }
}
