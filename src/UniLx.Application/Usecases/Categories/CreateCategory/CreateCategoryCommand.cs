using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Categories.CreateCategory
{
    public class CreateCategoryCommand : ICommand<IResult>
    {
        public string Root { get; set; }
        public string Name { get; set; }
        public string NameInPtBr { get; set; }
        public string? Description { get; set; }

        public CreateCategoryCommand(string root, string name, string nameInPtBr, string? description)
        {
            Root = root;
            Name = name;
            NameInPtBr = nameInPtBr;
            Description = description;
        }
    }

    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Root)
                .NotNull()
                .WithMessage("Advertisement type must be provided.")
                .Must(p => AdvertisementType.TryFromName(p, true, out _))
                .WithMessage($"Invalid root type. Supported types are: {string.Join(", ", AdvertisementType.List)}");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .MaximumLength(100)
                .WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.NameInPtBr)
                .NotEmpty()
                .WithMessage("Name in Portuguese cannot be empty.")
                .MaximumLength(100)
                .WithMessage("Name in Portuguese must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(256)
                .WithMessage("Description must not exceed 256 characters.");
        }
    }
}
