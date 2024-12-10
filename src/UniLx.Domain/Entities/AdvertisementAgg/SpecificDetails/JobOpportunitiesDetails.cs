using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class JobOpportunitiesDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.JobOpportunities;

        public string Position { get; private set; }
        public string Company { get; private set; }
        public int? Salary { get; private set; }
        public bool IsSalaryDisclosed { get; private set; }
        public WorkLocationType WorkLocation { get; private set; }
        public EmploymentType EmploymentType { get; private set; }
        public string? ExperienceLevel { get; private set; }
        public List<string>? Skills { get; private set; }
        public List<string>? Benefits { get; private set; }
        public bool RelocationHelp { get; private set; }
        public DateTime? ApplicationDeadline { get; private set; }
        public ContactInformation ContactInfo { get; private set; }

        private JobOpportunitiesDetails() : base()
        { }

        public JobOpportunitiesDetails(
            string? title,
            string? description,
            string? position,
            string? company,
            int? salary,
            bool isSalaryDisclosed,
            string? workLocation,
            string? employmentType,
            string? experienceLevel,
            List<string>? skills,
            List<string>? benefits,
            bool relocationHelp,
            DateTime? applicationDeadline,
            ContactInformation? contactInformation
        ) : base(title, description, null)
        {
            SetPosition(position);
            SetCompany(company);
            SetSalary(salary, isSalaryDisclosed);
            SetWorkLocation(workLocation);
            SetEmploymentType(employmentType);
            ExperienceLevel = experienceLevel;
            SetSkills(skills);
            SetBenefits(benefits);
            RelocationHelp = relocationHelp;
            SetApplicationDeadline(applicationDeadline);
            SetContactInformation(contactInformation);
        }

        private void SetPosition(string? position)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(position), "Position cannot be null or empty.");
            DomainException.ThrowIf(position!.Length > 100, "Position must be 100 characters or less.");
            Position = position;
        }

        private void SetCompany(string? company)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(company), "Company cannot be null or empty.");
            DomainException.ThrowIf(company!.Length > 100, "Company must be 100 characters or less.");
            Company = company;
        }

        private void SetSalary(int? salary, bool isSalaryDisclosed)
        {
            if (!isSalaryDisclosed && salary == null)
            {
                throw new DomainException("Either a salary must be provided or IsSalaryDisclosed must be true.");
            }

            Salary = salary;
            IsSalaryDisclosed = isSalaryDisclosed;
        }

        private void SetWorkLocation(string? workLocation)
        {
            var isValid = WorkLocationType.TryFromName(workLocation, true, out var locationType);
            DomainException.ThrowIf(!isValid, $"Invalid work location. Possible values are: {string.Join(", ", WorkLocationType.List.Select(w => w.Name))}.");
            WorkLocation = locationType!;
        }

        private void SetEmploymentType(string? employmentType)
        {
            var isValid = EmploymentType.TryFromName(employmentType, true, out var employmentTypeValue);
            DomainException.ThrowIf(!isValid, $"Invalid employment type. Possible values are: {string.Join(", ", EmploymentType.List.Select(e => e.Name))}.");
            EmploymentType = employmentTypeValue!;
        }

        private void SetSkills(List<string>? skills)
        {
            if (skills != null)
            {
                DomainException.ThrowIf(skills.Any(skill => string.IsNullOrWhiteSpace(skill)), "Skills cannot contain null or empty values.");
                DomainException.ThrowIf(skills.Any(skill => skill.Length > 50), "Each skill must be 50 characters or less.");
            }
            Skills = skills;
        }

        private void SetBenefits(List<string>? benefits)
        {
            if (benefits != null)
            {
                DomainException.ThrowIf(benefits.Any(benefit => string.IsNullOrWhiteSpace(benefit)), "Benefits cannot contain null or empty values.");
                DomainException.ThrowIf(benefits.Any(benefit => benefit.Length > 50), "Each benefit must be 50 characters or less.");
            }
            Benefits = benefits;
        }

        private void SetApplicationDeadline(DateTime? applicationDeadline)
        {
            if (applicationDeadline.HasValue)
            {
                DomainException.ThrowIf(applicationDeadline.Value <= DateTime.UtcNow, "Application deadline must be in the future.");
            }
            ApplicationDeadline = applicationDeadline;
        }

        private void SetContactInformation(ContactInformation? contactInformation)
        {
            DomainException.ThrowIf(contactInformation is null, "Contact information must be provided.");
            ContactInfo = contactInformation!;
        }
    }
}
