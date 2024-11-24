using FluentValidation;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands
{
    public class CreateJobOpportunitiesDetailsCommand
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Position { get; set; }
        public string Company { get; set; }
        public int? Salary { get; set; }
        public bool IsSalaryDisclosed { get; set; }
        public string WorkLocation { get; set; }
        public string EmploymentType { get; set; }
        public string? ExperienceLevel { get; set; }
        public List<string>? Skills { get; set; }
        public List<string>? Benefits { get; set; }
        public bool RelocationHelp { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        public CreateContactInformationCommand? ContactInformation { get; set; }

        public CreateJobOpportunitiesDetailsCommand(
            string? title,
            string? description,
            string position,
            string company,
            int? salary,
            bool isSalaryDisclosed,
            string workLocation,
            string employmentType,
            string? experienceLevel,
            List<string>? skills,
            List<string>? benefits,
            bool relocationHelp,
            DateTime? applicationDeadline,
            CreateContactInformationCommand? contactInformation)
        {
            Title = title;
            Description = description;
            Position = position;
            Company = company;
            Salary = salary;
            IsSalaryDisclosed = isSalaryDisclosed;
            WorkLocation = workLocation;
            EmploymentType = employmentType;
            ExperienceLevel = experienceLevel;
            Skills = skills;
            Benefits = benefits;
            RelocationHelp = relocationHelp;
            ApplicationDeadline = applicationDeadline;
            ContactInformation = contactInformation;
        }
    }

    public class CreateJobOpportunitiesDetailsCommandValidator : AbstractValidator<CreateJobOpportunitiesDetailsCommand>
    {
        public CreateJobOpportunitiesDetailsCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(256).WithMessage("Title must not exceed 256 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(512).WithMessage("Description must not exceed 512 characters.")
                .When(x => x.Description != null);

            RuleFor(x => x.Position)
                .NotEmpty().WithMessage("Position is required.")
                .MaximumLength(100).WithMessage("Position must not exceed 100 characters.");

            RuleFor(x => x.Company)
                .NotEmpty().WithMessage("Company is required.")
                .MaximumLength(100).WithMessage("Company must not exceed 100 characters.");

            RuleFor(x => x.WorkLocation)
                .NotEmpty().WithMessage("Work location is required.")
                .Must(location => WorkLocationType.TryFromName(location, true, out _))
                .WithMessage($"Invalid work location. Valid values are: {string.Join(", ", WorkLocationType.List.Select(w => w.Name))}");

            RuleFor(x => x.EmploymentType)
                .NotEmpty().WithMessage("Employment type is required.")
                .Must(type => EmploymentType.TryFromName(type, true, out _))
                .WithMessage($"Invalid employment type. Valid values are: {string.Join(", ", EmploymentType.List.Select(e => e.Name))}");

            RuleFor(x => x.Salary)
                .GreaterThanOrEqualTo(0).WithMessage("Salary must be greater than or equal to 0.")
                .When(x => x.IsSalaryDisclosed == false);

            RuleForEach(x => x.Skills).ChildRules(skill =>
            {
                skill.RuleFor(x => x)
                    .NotEmpty().WithMessage("Skill cannot be empty.")
                    .MaximumLength(50).WithMessage("Each skill must not exceed 50 characters.");
            }).When(x => x.Skills != null);

            RuleForEach(x => x.Benefits).ChildRules(benefit =>
            {
                benefit.RuleFor(x => x)
                    .NotEmpty().WithMessage("Benefit cannot be empty.")
                    .MaximumLength(50).WithMessage("Each benefit must not exceed 50 characters.");
            }).When(x => x.Benefits != null);

            RuleFor(x => x.ApplicationDeadline)
                .GreaterThan(DateTime.UtcNow).WithMessage("Application deadline must be in the future.")
                .When(x => x.ApplicationDeadline.HasValue);

            RuleFor(x => x.ContactInformation)
                .NotNull().WithMessage("Contact information is required.")
                .SetValidator(new CreateContactInformationCommandValidator()!);
        }
    }
}
