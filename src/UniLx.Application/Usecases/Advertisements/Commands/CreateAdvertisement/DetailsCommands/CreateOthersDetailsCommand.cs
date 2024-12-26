using FluentValidation;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands
{
    public class CreateOthersDetailsCommand
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? Condition { get; set; }
        public string? Brand { get; set; }
        public List<string>? Features { get; set; }
        public DateTime? WarrantyUntil { get; set; }

        public CreateOthersDetailsCommand(
            string? title,
            string? description,
            int? price,
            string? condition,
            string? brand,
            List<string>? features,
            DateTime? warrantyUntil
        )
        {
            Title = title;
            Description = description;
            Price = price;
            Condition = condition;
            Brand = brand;
            Features = features;
            WarrantyUntil = warrantyUntil;
        }
    }

    public class CreateOthersDetailsCommandValidator : AbstractValidator<CreateOthersDetailsCommand>
    {
        public CreateOthersDetailsCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Price)                
                .GreaterThan(0).WithMessage("Price must be greater than zero.")
                .LessThanOrEqualTo(100_000_000).WithMessage("Price must be less than or equal to 100,000,000.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.Condition)
                .Must(conditionName => ProductCondition.TryFromName(conditionName, true, out _))
                .WithMessage($"Invalid product condition. Possible values are: {string.Join(", ", ProductCondition.List.Select(c => c.Name))}.")
                .When(x => !string.IsNullOrWhiteSpace(x.Condition));

            RuleFor(x => x.Brand)
                .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.")
                .When(x => !string.IsNullOrWhiteSpace(x.Brand));

            RuleForEach(x => x.Features)
                .NotEmpty().WithMessage("Feature cannot be empty.")
                .MaximumLength(50).WithMessage("Each feature must not exceed 50 characters.")
                .When(x => x.Features != null && x.Features.Count != 0);

            RuleFor(x => x.WarrantyUntil)
                .GreaterThan(DateTime.UtcNow).WithMessage("WarrantyUntil must be in the future.")
                .When(x => x.WarrantyUntil.HasValue);
        }
    }
}
