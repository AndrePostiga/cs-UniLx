using System.Text.RegularExpressions;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class ElectronicsDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.Electronics;

        public string ProductType { get; private set; }   // e.g., Smartphone, Laptop, PC, Headphones
        public string Brand { get; private set; }         // e.g., Samsung, Apple, Sony
        public string? Model { get; private set; }        // e.g., iPhone 14, PlayStation 5
        public string? StorageCapacity { get; private set; } // e.g., 128Gb, 1Tb
        public string? Memory { get; private set; }          // e.g., 16Gb, 512Mb
        public string? Processor { get; private set; }    // e.g., Intel i7, AMD Ryzen 9
        public string? GraphicsCard { get; private set; } // e.g., NVIDIA RTX 3080
        public float? BatteryLife { get; private set; }    // Battery life as a fraction between 0.0 and 1.0
        public bool HasWarranty { get; private set; }      // Indicates if a warranty is included
        public DateTime? WarrantyUntil { get; private set; } // Warranty expiration date
        public List<string>? Features { get; private set; }   // List of additional features (e.g., 4K Display, Bluetooth 5.0)
        public ProductCondition Condition { get; private set; }      // Smart enum for product condition
        public bool? IncludesOriginalBox { get; private set; } // Whether the original box is included
        public bool IncludesAccessories => Accessories?.Count > 0; // Derived property based on Accessories
        public List<string>? Accessories { get; private set; } // List of included accessories

        private ElectronicsDetails() : base() { }

        public ElectronicsDetails(
            string title,
            string? description,
            int? price,
            string productType,
            string brand,
            string? model,
            string? storageCapacity,
            string? memory,
            string? processor,
            string? graphicsCard,
            float? batteryLife,
            DateTime? warrantyUntil,
            List<string>? features,
            string condition,
            bool? includesOriginalBox,
            List<string>? accessories
        ) : base(title, description, price)
        {
            SetPrice(price);
            SetProductType(productType);
            SetBrand(brand);
            Model = model;
            SetStorageCapacity(storageCapacity);
            SetMemory(memory);
            SetProcessor(processor);
            SetGraphicsCard(graphicsCard);
            SetBatteryLife(batteryLife);
            SetWarranty(warrantyUntil);
            HasWarranty = warrantyUntil > DateTime.Now;
            SetFeatures(features);
            SetCondition(condition);
            IncludesOriginalBox = includesOriginalBox;
            SetAccessories(accessories);
        }

        private void SetCondition(string? condition)
        {
            var hasRestrictionType = ProductCondition.TryFromName(condition, ignoreCase: true, out var productCondition);
            DomainException.ThrowIf(hasRestrictionType == false, $"Invalid product condition, possible values are {string.Join(",", ProductCondition.List)}.");
            Condition = productCondition;
        }

        private void SetPrice(int? price)
        {
            DomainException.ThrowIf(!price.HasValue, "Price must have a value.");
            DomainException.ThrowIf(price <= 0, "Price must be greater than 0.");
            DomainException.ThrowIf(price > 100_000_000, "Price cannot exceed R$ 1,000,000.00.");
            Price = price;
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

        private void SetStorageCapacity(string? storageCapacity)
        {
            if (!string.IsNullOrWhiteSpace(storageCapacity))
            {
                var regex = new Regex(@"^\d+(KB|MB|GB|TB)$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
                DomainException.ThrowIf(!regex.IsMatch(storageCapacity), "StorageCapacity must start with numbers and end with Kb, Mb, Gb, or Tb.");
            }
            StorageCapacity = storageCapacity;
        }

        private void SetMemory(string? memory)
        {
            if (!string.IsNullOrWhiteSpace(memory))
            {
                var regex = new Regex(@"^\d+(KB|MB|GB|TB)$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(100));
                DomainException.ThrowIf(!regex.IsMatch(memory), "Memory must start with numbers and end with Kb, Mb, Gb, or Tb.");
            }
            Memory = memory;
        }

        private void SetProcessor(string? processor)
        {
            if (!string.IsNullOrWhiteSpace(processor))
            {
                DomainException.ThrowIf(processor.Length > 100, "Processor must be 100 characters or less.");
            }
            Processor = processor;
        }

        private void SetGraphicsCard(string? graphicsCard)
        {
            if (!string.IsNullOrWhiteSpace(graphicsCard))
            {
                DomainException.ThrowIf(graphicsCard.Length > 100, "GraphicsCard must be 100 characters or less.");
            }
            GraphicsCard = graphicsCard;
        }

        private void SetBatteryLife(float? batteryLife)
        {
            if (batteryLife.HasValue)
            {
                DomainException.ThrowIf(batteryLife.Value < 0.0f || batteryLife.Value > 1.0f, "BatteryLife must be between 0.0 and 1.0.");
            }
            BatteryLife = batteryLife;
        }

        private void SetWarranty(DateTime? warrantyUntil)
        {
            if (warrantyUntil.HasValue)
                DomainException.ThrowIf(warrantyUntil.Value <= DateTime.UtcNow, "WarrantyUntil must be in the future.");
            
            WarrantyUntil = warrantyUntil;
        }

        private void SetFeatures(List<string>? features)
        {
            if (features != null)
            {
                DomainException.ThrowIf(features.Any(f => string.IsNullOrWhiteSpace(f)), "Feature cannot be null or empty.");
                DomainException.ThrowIf(features.Any(f => f.Length > 50), "Each feature must be 50 characters or less.");
            }
            Features = features;
        }

        private void SetAccessories(List<string>? accessories)
        {
            if (accessories != null)
            {
                DomainException.ThrowIf(accessories.Any(a => string.IsNullOrWhiteSpace(a)), "Accessory cannot be null or empty.");
                DomainException.ThrowIf(accessories.Any(a => a.Length > 50), "Each accessory must be 50 characters or less.");
            }
            Accessories = accessories;
        }
    }

}
