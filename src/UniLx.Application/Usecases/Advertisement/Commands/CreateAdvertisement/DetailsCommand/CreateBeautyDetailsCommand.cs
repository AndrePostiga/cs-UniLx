using FluentValidation;

namespace UniLx.Application.Usecases.Advertisement.Commands.CreateAdvertisement.DetailsCommand
{
    public class CreateBeautyDetailsCommand
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? ProductType { get; set; }
        public string? Brand { get; set; }
        public string? SkinType { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public List<string>? Ingredients { get; set; }
        public bool? IsOrganic { get; set; }

        public CreateBeautyDetailsCommand(string? title, string? description, int? price, string? productType,
            string? brand, string? skinType, DateTime? expirationDate, List<string>? ingredients, bool? isOrganic)
        {
            Title = title;
            Description = description;
            Price = price;
            ProductType = productType;
            Brand = brand;
            SkinType = skinType;
            ExpirationDate = expirationDate;
            Ingredients = ingredients;
            IsOrganic = isOrganic;
        }
    }

    public class CreateBeautyDetailsCommandValidator : AbstractValidator<CreateBeautyDetailsCommand>
    {
        public CreateBeautyDetailsCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.")
                .LessThanOrEqualTo(100_000_000).WithMessage("Price must be less than or equal to 100,000,000.")
                .When(x => x.Price.HasValue);

            RuleFor(x => x.ProductType)
                .NotEmpty().WithMessage("ProductType is required.")
                .MaximumLength(100).WithMessage("ProductType must not exceed 100 characters.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.");

            RuleFor(x => x.SkinType)
                .MaximumLength(50).WithMessage("SkinType must not exceed 50 characters.");

            RuleFor(x => x.ExpirationDate)
                .GreaterThan(DateTime.UtcNow).WithMessage("ExpirationDate cannot be in the past.")
                .When(x => x.ExpirationDate.HasValue);

            RuleForEach(x => x.Ingredients).ChildRules(ingredients =>
            {
                ingredients.RuleFor(x => x)
                    .NotEmpty().WithMessage("Ingredient cannot be empty.")
                    .MaximumLength(50).WithMessage("Each ingredient must not exceed 50 characters.");
            }).When(x => x.Ingredients != null);
        }
    }
}
