using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class FashionDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.Fashion;

        public string ClothingType { get; private set; }  // e.g., T-shirt, Dress, Jacket
        public string Brand { get; private set; }         // e.g., Nike, Gucci
        public List<FashionSize> Sizes { get; private set; } // List of sizes (must have at least one)
        public FashionGender Gender { get; private set; }    // e.g., Male, Female, Unisex
        public List<string>? Colors { get; private set; } // List of colors (e.g., Red, Blue)
        public List<string>? Materials { get; private set; } // List of materials (e.g., Cotton, Polyester)
        public List<string>? Features { get; private set; } // e.g., Waterproof, Breathable
        public string? Designer { get; private set; }     // Designer name, if applicable
        public bool? IsHandmade { get; private set; }     // Whether the item is handmade
        public DateTime? ReleaseDate { get; private set; } // When the fashion item was released
        public bool? IsSustainable { get; private set; }  // Whether it’s eco-friendly or sustainable

        private FashionDetails() : base() { }

        public FashionDetails(
            string? title,
            string? description,
            int? price,
            string clothingType,
            string brand,
            List<string> sizes,
            string gender,
            List<string>? colors = null,
            List<string>? materials = null,
            List<string>? features = null,
            string? designer = null,
            bool? isHandmade = null,
            DateTime? releaseDate = null,
            bool? isSustainable = null) : base(title, description, price)
        {
            SetClothingType(clothingType);
            SetBrand(brand);
            SetSizes(sizes);
            SetGender(gender);
            SetColors(colors);
            SetMaterials(materials);
            SetFeatures(features);
            Designer = designer;
            IsHandmade = isHandmade;
            ReleaseDate = releaseDate;
            IsSustainable = isSustainable;
        }

        private void SetClothingType(string clothingType)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(clothingType), "ClothingType cannot be null or empty.");
            DomainException.ThrowIf(clothingType.Length > 100, "ClothingType must be 100 characters or less.");
            ClothingType = clothingType;
        }

        private void SetBrand(string brand)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(brand), "Brand cannot be null or empty.");
            DomainException.ThrowIf(brand.Length > 100, "Brand must be 100 characters or less.");
            Brand = brand;
        }

        private void SetSizes(List<string> sizes)
        {
            DomainException.ThrowIf(sizes is null || sizes.Count == 0, "At least one size must be provided.");

            var validatedSizes = new List<FashionSize>();
            foreach (var size in sizes!)
            {
                DomainException.ThrowIf(string.IsNullOrWhiteSpace(size), "Size cannot contain null or empty values.");

                var isValidSize = FashionSize.TryFromName(size, ignoreCase: true, out var fashionSize);
                DomainException.ThrowIf(!isValidSize, $"Invalid size '{size}'. Possible values are: {string.Join(", ", FashionSize.List.Select(s => s.Name))}");

                validatedSizes.Add(fashionSize);
            }

            Sizes = validatedSizes;
        }

        private void SetGender(string gender)
        {
            var hasGenderType = FashionGender.TryFromName(gender, ignoreCase: true, out var genderType);
            DomainException.ThrowIf(hasGenderType == false, $"Invalid fashion gender type, possible values are {string.Join(",", FashionGender.List)}.");
            Gender = genderType;
        }

        private void SetColors(List<string>? colors)
        {
            if (colors != null)
            {
                DomainException.ThrowIf(colors.Any(c => string.IsNullOrWhiteSpace(c)), "Colors cannot contain null or empty values.");
                DomainException.ThrowIf(colors.Any(c => c.Length > 50), "Each color must be 50 characters or less.");
            }
            Colors = colors;
        }

        private void SetMaterials(List<string>? materials)
        {
            if (materials != null)
            {
                DomainException.ThrowIf(materials.Any(m => string.IsNullOrWhiteSpace(m)), "Materials cannot contain null or empty values.");
                DomainException.ThrowIf(materials.Any(m => m.Length > 50), "Each material must be 50 characters or less.");
            }
            Materials = materials;
        }

        private void SetFeatures(List<string>? features)
        {
            if (features != null)
            {
                DomainException.ThrowIf(features.Any(f => string.IsNullOrWhiteSpace(f)), "Features cannot contain null or empty values.");
                DomainException.ThrowIf(features.Any(f => f.Length > 50), "Each feature must be 50 characters or less.");
            }
            Features = features;
        }
    }

}
