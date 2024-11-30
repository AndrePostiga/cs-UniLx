using System.Reflection;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class JobOpportunitiesDetailsTests
    {
        [Theory]
        [InlineData("Remote", "Full_Time")]
        [InlineData("On_Site", "Part_Time")]
        [InlineData("Hybrid", "Contract")]
        public void JobOpportunitiesDetails_Should_Create_Valid_Instance(string workLocation, string employmentType)
        {
            // Arrange
            var title = "Software Engineer";
            var description = "A great opportunity for a skilled software engineer.";
            var position = "Backend Developer";
            var company = "TechCorp";
            var salary = 120000;
            var isSalaryDisclosed = true;
            var experienceLevel = "Mid-Level";
            var skills = new List<string> { "C#", "ASP.NET", "SQL" };
            var benefits = new List<string> { "Health Insurance", "Remote Work" };
            var relocationHelp = true;
            var applicationDeadline = DateTime.UtcNow.AddMonths(1);
            var contactInfo = new ContactInformation(new Phone("55", "11", "912345678"), new Email("hr@techcorp.com"), "https://techcorp.com");

            // Act
            var details = new JobOpportunitiesDetails(
                title, description, position, company, salary, isSalaryDisclosed, workLocation,
                employmentType, experienceLevel, skills, benefits, relocationHelp, applicationDeadline, contactInfo);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(position, details.Position);
            Assert.Equal(company, details.Company);
            Assert.Equal(salary, details.Salary);
            Assert.Equal(isSalaryDisclosed, details.IsSalaryDisclosed);
            Assert.Equal(WorkLocationType.FromName(workLocation, true), details.WorkLocation);
            Assert.Equal(EmploymentType.FromName(employmentType, true), details.EmploymentType);
            Assert.Equal(experienceLevel, details.ExperienceLevel);
            Assert.Equal(skills, details.Skills);
            Assert.Equal(benefits, details.Benefits);
            Assert.Equal(relocationHelp, details.RelocationHelp);
            Assert.Equal(applicationDeadline, details.ApplicationDeadline);
            Assert.Equal(contactInfo, details.ContactInfo);
        }

        [Fact]
        public void JobOpportunitiesDetails_Should_Throw_When_Position_Is_Null_Or_Empty()
        {
            // Arrange
            string? invalidPosition = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new JobOpportunitiesDetails("Title", "Description", invalidPosition, "Company", 100000, true, "Remote", "FullTime", null, null, null, false, null, null));
        }

        [Fact]
        public void JobOpportunitiesDetails_Should_Throw_When_Company_Is_Null_Or_Empty()
        {
            // Arrange
            string? invalidCompany = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new JobOpportunitiesDetails("Title", "Description", "Position", invalidCompany, 100000, true, "Remote", "FullTime", null, null, null, false, null, null));
        }

        [Fact]
        public void JobOpportunitiesDetails_Should_Throw_When_WorkLocation_Is_Invalid()
        {
            // Arrange
            var invalidWorkLocation = "InvalidLocation";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new JobOpportunitiesDetails("Title", "Description", "Position", "Company", 100000, true, invalidWorkLocation, "FullTime", null, null, null, false, null, null));
        }

        [Fact]
        public void JobOpportunitiesDetails_Should_Throw_When_EmploymentType_Is_Invalid()
        {
            // Arrange
            var invalidEmploymentType = "InvalidType";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new JobOpportunitiesDetails("Title", "Description", "Position", "Company", 100000, true, "Remote", invalidEmploymentType, null, null, null, false, null, null));
        }

        [Fact]
        public void JobOpportunitiesDetails_Should_Throw_When_Salary_And_Disclosure_Are_Invalid()
        {
            // Arrange
            int? salary = null;
            var isSalaryDisclosed = false;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new JobOpportunitiesDetails("Title", "Description", "Position", "Company", salary, isSalaryDisclosed, "Remote", "FullTime", null, null, null, false, null, null));
        }

        [Fact]
        public void JobOpportunitiesDetails_Should_Throw_When_ApplicationDeadline_Is_In_The_Past()
        {
            // Arrange
            var pastDeadline = DateTime.UtcNow.AddDays(-1);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new JobOpportunitiesDetails("Title", "Description", "Position", "Company", 100000, true, "Remote", "FullTime", null, null, null, false, pastDeadline, null));
        }

        [Fact]
        public void JobOpportunitiesDetails_Protected_Default_Constructor_Should_Work()
        {
            // Act
            var constructor = typeof(JobOpportunitiesDetails).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var details = (JobOpportunitiesDetails)constructor.Invoke(null);

            // Assert
            Assert.NotNull(details);
            Assert.Null(details.Position);
            Assert.Null(details.Company);
            Assert.Null(details.Salary);
            Assert.Null(details.ExperienceLevel);
            Assert.Null(details.Skills);
            Assert.Null(details.Benefits);
            Assert.Null(details.ApplicationDeadline);
            Assert.Null(details.ContactInfo);
        }
    }
}
