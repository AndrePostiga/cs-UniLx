using FluentValidation;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands
{
    public class CreateFashionDetailsCommand
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? ClothingType { get; set; }
        public string? Brand { get; set; }
        public List<string>? Sizes { get; set; }
        public string? Gender { get; set; }
        public List<string>? Colors { get; set; }
        public List<string>? Materials { get; set; }
        public List<string>? Features { get; set; }
        public string? Designer { get; set; }
        public bool? IsHandmade { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public bool? IsSustainable { get; set; }

        public CreateFashionDetailsCommand(string? title, string? description, int? price, string? clothingType, 
            string? brand, List<string>? sizes, string? gender, List<string>? colors, List<string>? materials, 
            List<string>? features, string? designer, bool? isHandmade, DateTime? releaseDate, bool? isSustainable)
        {
            Title = title;
            Description = description;
            Price = price;
            ClothingType = clothingType;
            Brand = brand;
            Sizes = sizes;
            Gender = gender;
            Colors = colors;
            Materials = materials;
            Features = features;
            Designer = designer;
            IsHandmade = isHandmade;
            ReleaseDate = releaseDate;
            IsSustainable = isSustainable;
        }
    }

    public class CreateFashionDetailsCommandValidator : AbstractValidator<CreateFashionDetailsCommand>
    {
        public CreateFashionDetailsCommandValidator()
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

            RuleFor(x => x.ClothingType)
                .NotEmpty().WithMessage("ClothingType is required.")
                .MaximumLength(100).WithMessage("ClothingType must not exceed 100 characters.");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required.")
                .MaximumLength(100).WithMessage("Brand must not exceed 100 characters.");

            RuleFor(x => x.Sizes)
                .NotNull().WithMessage("At least one size must be provided.")
                .Must(sizes => sizes!.All(size => FashionSize.TryFromName(size, true, out _)))
                .WithMessage($"Invalid size. Possible values are: {string.Join(", ", FashionSize.List.Select(s => s.Name))}");

            RuleFor(x => x.Gender)
                .NotEmpty().WithMessage("Gender is required.")
                .Must(gender => FashionGender.TryFromName(gender, true, out _))
                .WithMessage($"Invalid gender. Possible values are: {string.Join(", ", FashionGender.List.Select(g => g.Name))}");

            RuleForEach(x => x.Colors).ChildRules(color =>
            {
                color.RuleFor(c => c)
                    .NotEmpty().WithMessage("Color cannot be empty.")
                    .MaximumLength(50).WithMessage("Color must not exceed 50 characters.");
            }).When(x => x.Colors != null);

            RuleForEach(x => x.Materials).ChildRules(material =>
            {
                material.RuleFor(m => m)
                    .NotEmpty().WithMessage("Material cannot be empty.")
                    .MaximumLength(50).WithMessage("Material must not exceed 50 characters.");
            }).When(x => x.Materials != null);

            RuleForEach(x => x.Features).ChildRules(feature =>
            {
                feature.RuleFor(f => f)
                    .NotEmpty().WithMessage("Feature cannot be empty.")
                    .MaximumLength(50).WithMessage("Feature must not exceed 50 characters.");
            }).When(x => x.Features != null);

            RuleFor(x => x.Designer)
                .MaximumLength(100).WithMessage("Designer name must not exceed 100 characters.")
                .When(x => x.Designer != null);

            RuleFor(x => x.ReleaseDate)
                .GreaterThan(DateTime.MinValue).WithMessage("Invalid ReleaseDate.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("ReleaseDate cannot be in the future.")
                .When(x => x.ReleaseDate.HasValue);
        }
    }
}
