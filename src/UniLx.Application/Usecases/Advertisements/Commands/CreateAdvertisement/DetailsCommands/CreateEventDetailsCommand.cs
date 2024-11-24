using FluentValidation;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommand
{
    public class CreateEventDetailsCommand
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public string? EventType { get; set; }
        public DateTime? EventDate { get; set; }
        public string? Organizer { get; set; }
        public string? AgeRestriction { get; set; }
        public string? DressCode { get; set; }
        public List<string>? Highlights { get; set; }
        public bool? IsOnline { get; set; }
        public CreateContactInformationCommand? ContactInformation { get; set; }

        public CreateEventDetailsCommand(
            string? title, string? description, int? price, string? eventType,
            DateTime? eventDate, string? organizer, string? ageRestriction,
            string? dressCode, List<string>? highlights, bool? isOnline,
            CreateContactInformationCommand? contactInformation)
        {
            Title = title;
            Description = description;
            Price = price;
            EventType = eventType;
            EventDate = eventDate;
            Organizer = organizer;
            AgeRestriction = ageRestriction;
            DressCode = dressCode;
            Highlights = highlights;
            IsOnline = isOnline;
            ContactInformation = contactInformation;
        }
    }

    public class CreateEventDetailsCommandValidator : AbstractValidator<CreateEventDetailsCommand>
    {
        public CreateEventDetailsCommandValidator()
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

            RuleFor(x => x.EventType)
                .NotEmpty().WithMessage("EventType is required.")
                .MaximumLength(100).WithMessage("EventType must not exceed 100 characters.");

            RuleFor(x => x.EventDate)
                .NotNull().WithMessage("EventDate is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("EventDate cannot be in the past.");


            RuleFor(x => x.AgeRestriction)
                .NotEmpty().WithMessage("Age restriction is required.")
                .Must(ageRestriction => AgeRestriction.TryFromName(ageRestriction, true, out _))
                .WithMessage($"Invalid age restriction. Supported ages restrictions are: {string.Join(", ", AgeRestriction.List)}");

            RuleFor(x => x.DressCode)
                .MaximumLength(100).WithMessage("DressCode must not exceed 100 characters.")
                .When(x => x.DressCode != null);

            RuleForEach(x => x.Highlights).ChildRules(highlight =>
            {
                highlight.RuleFor(x => x)
                    .NotEmpty().WithMessage("Highlight cannot be empty.")
                    .MaximumLength(100).WithMessage("Each highlight must not exceed 100 characters.");
            }).When(x => x.Highlights != null);

            RuleFor(x => x.ContactInformation)
                .NotNull().WithMessage("Contact information must be provided.")
                .SetValidator(new CreateContactInformationCommandValidator()!);
        }
    }
}
