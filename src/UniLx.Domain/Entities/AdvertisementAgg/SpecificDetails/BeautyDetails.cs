using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class BeautyDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.Beauty;

        public string? ProductType { get; set; }   // e.g., Skincare, Makeup
        public string? Brand { get; set; }         // e.g., L'Oreal, Maybelline
        public string? SkinType { get; set; }      // e.g., Dry, Oily, Combination
        public DateTime? ExpirationDate { get; set; }  // Expiration date for beauty products
        public List<string>? Ingredients { get; set; }  // List of ingredients in the product
        public bool? IsOrganic { get; set; }       // Whether the product is organic

        public BeautyDetails(string? title, string? description, int? price, string? productType, 
            string? brand, string? skinType, DateTime? expirationDate, List<string>? ingredients, 
            bool? isOrganic) : base(title, description, price)
        {
            SetProductType(productType);
            SetBrand(brand);
            SetSkinType(skinType);
            SetExpirationDate(expirationDate);
            SetIngredients(ingredients);
            IsOrganic = isOrganic;
        }

        private void SetProductType(string productType)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(productType), "ProductType cannot be null or empty.");
            DomainException.ThrowIf(productType.Length > 100, "ProductType must be 100 characters or less.");
            ProductType = productType;
        }

        private void SetBrand(string brand)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(brand), "Brand cannot be null or empty.");
            DomainException.ThrowIf(brand.Length > 100, "Brand must be 100 characters or less.");
            Brand = brand;
        }

        private void SetSkinType(string skinType)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(skinType), "SkinType cannot be null or empty.");
            DomainException.ThrowIf(skinType.Length > 50, "SkinType must be 50 characters or less.");
            SkinType = skinType;
        }

        private void SetExpirationDate(DateTime? expirationDate)
        {
            if (expirationDate.HasValue)
            {
                DomainException.ThrowIf(expirationDate.Value < DateTime.UtcNow, "ExpirationDate cannot be in the past.");
                ExpirationDate = expirationDate;
            }
        }

        private void SetIngredients(List<string>? ingredients)
        {
            if (ingredients != null)
            {
                DomainException.ThrowIf(ingredients.Count == 0, "Ingredients list cannot be empty.");
                foreach (var ingredient in ingredients)
                {
                    DomainException.ThrowIf(string.IsNullOrWhiteSpace(ingredient), "Ingredient cannot be null or empty.");
                    DomainException.ThrowIf(ingredient.Length > 50, "Each ingredient must be 50 characters or less.");
                }
            }
            Ingredients = ingredients;
        }
    }

}
