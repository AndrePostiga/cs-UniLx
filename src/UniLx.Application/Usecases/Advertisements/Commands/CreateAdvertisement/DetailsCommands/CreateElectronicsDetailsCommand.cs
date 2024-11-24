using FluentValidation;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands
{
    public class CreateElectronicsDetailsCommand
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? ProductType { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? StorageCapacity { get; set; }
        public string? Memory { get; set; }
        public string? Processor { get; set; }
        public string? GraphicsCard { get; set; }
        public float? BatteryLife { get; set; }
        public DateTime? WarrantyUntil { get; set; }
        public List<string>? Features { get; set; }
        public string? Condition { get; set; }
        public bool? IncludesOriginalBox { get; set; }
        public List<string>? Accessories { get; set; }

        public CreateElectronicsDetailsCommand(
            string? title,
            string? description,
            int? price,
            string? productType,
            string? brand,
            string? model,
            string? storageCapacity,
            string? memory,
            string? processor,
            string? graphicsCard,
            float? batteryLife,
            DateTime? warrantyUntil,
            List<string>? features,
            string? condition,
            bool? includesOriginalBox,
            List<string>? accessories)
        {
            Title = title;
            Description = description;
            Price = price;
            ProductType = productType;
            Brand = brand;
            Model = model;
            StorageCapacity = storageCapacity;
            Memory = memory;
            Processor = processor;
            GraphicsCard = graphicsCard;
            BatteryLife = batteryLife;
            WarrantyUntil = warrantyUntil;
            Features = features;
            Condition = condition;
            IncludesOriginalBox = includesOriginalBox;
            Accessories = accessories;
        }
    }

    public class CreateElectronicsDetailsCommandValidator : AbstractValidator<CreateElectronicsDetailsCommand>
    {
        public CreateElectronicsDetailsCommandValidator()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Title is required.")
               .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0.")
                .LessThanOrEqualTo(100_000_000).WithMessage("Price must be less than or equal to 100,000,000.");

            RuleFor(x => x.ProductType)
                .NotEmpty().WithMessage("ProductType is required.")
                .MaximumLength(100).WithMessage("ProductType must not exceed 100 characters.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.");

            RuleFor(x => x.Model)
                .MaximumLength(100).WithMessage("Model must not exceed 100 characters.")
                .When(x => x.Model != null);

            RuleFor(x => x.StorageCapacity)
                .Matches(@"^\d+(KB|MB|GB|TB)$").WithMessage("StorageCapacity must be in the format: numbers followed by KB, MB, GB, or TB.")
                .When(x => x.StorageCapacity != null);

            RuleFor(x => x.Memory)
                .Matches(@"^\d+(KB|MB|GB|TB)$").WithMessage("Memory must be in the format: numbers followed by KB, MB, GB, or TB.")
                .When(x => x.Memory != null);

            RuleFor(x => x.Processor)
                .MaximumLength(100).WithMessage("Processor must not exceed 100 characters.")
                .When(x => x.Processor != null);

            RuleFor(x => x.GraphicsCard)
                .MaximumLength(100).WithMessage("GraphicsCard must not exceed 100 characters.")
                .When(x => x.GraphicsCard != null);

            RuleFor(x => x.BatteryLife)
                .InclusiveBetween(0.0f, 1.0f).WithMessage("BatteryLife must be between 0.0 and 1.0.")
                .When(x => x.BatteryLife.HasValue);

            RuleFor(x => x.WarrantyUntil)
                .GreaterThan(DateTime.UtcNow).WithMessage("WarrantyUntil must be in the future.")
                .When(x => x.WarrantyUntil.HasValue);

            RuleForEach(x => x.Features).ChildRules(feature =>
            {
                feature.RuleFor(f => f)
                    .NotEmpty().WithMessage("Feature cannot be empty.")
                    .MaximumLength(50).WithMessage("Each feature must not exceed 50 characters.");
            }).When(x => x.Features != null);

            RuleFor(x => x.Condition)
                .NotEmpty().WithMessage("Condition is required.")
                .Must(condition => ProductCondition.TryFromName(condition, true, out _))
                .WithMessage($"Invalid condition. Supported conditions are: {string.Join(", ", ProductCondition.List)}");

            RuleForEach(x => x.Accessories).ChildRules(accessory =>
            {
                accessory.RuleFor(a => a)
                    .NotEmpty().WithMessage("Accessory cannot be empty.")
                    .MaximumLength(50).WithMessage("Each accessory must not exceed 50 characters.");
            }).When(x => x.Accessories != null);
        }
    }
}
