using System.Reflection;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Exceptions;

namespace UniLx.Tests.Domain.AdvertisementAgg
{
    public class PetDetailsTests
    {
        [Fact]
        public void PetDetails_Protected_Default_Constructor_Should_Work()
        {
            // Act
            var constructor = typeof(PetDetails).GetConstructor(
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                Type.EmptyTypes,
                null);

            var details = (PetDetails)constructor!.Invoke(null);

            // Assert
            Assert.NotNull(details);
            Assert.Null(details.AnimalType);
            Assert.Null(details.Breed);
            Assert.Null(details.AccessoryType);
            Assert.Null(details.Materials);
            Assert.Null(details.AdoptionRequirements);
            Assert.Null(details.HealthStatus);
            Assert.Null(details.IsSterilized);
            Assert.Null(details.Price);
            Assert.Null(details.Gender);
            Assert.Null(details.IsVaccinated);
        }

        [Fact]
        public void PetDetails_Should_Create_Valid_Instance()
        {
            // Arrange
            var title = "Adorable Puppy for Sale";
            var description = "Healthy and playful puppy, perfect for families.";
            var price = 3000;
            var petType = "Sell";
            var animalType = "Dog";
            var age = 2;
            var breed = "Golden Retriever";
            var isVaccinated = true;
            var gender = "Male";
            var isExotic = false;
            var healthStatus = "Excellent";
            var isSterilized = true;

            // Act
            var details = new PetDetails(
                title, description, price, petType, animalType, age, breed, isVaccinated,
                gender, isExotic, null, null, null, healthStatus, isSterilized);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(title, details.Title);
            Assert.Equal(description, details.Description);
            Assert.Equal(price, details.Price);
            Assert.Equal(animalType, details.AnimalType);
            Assert.Equal(age, details.Age);
            Assert.Equal(breed, details.Breed);
            Assert.Equal(isVaccinated, details.IsVaccinated);
            Assert.Equal(isSterilized, details.IsSterilized);
            Assert.Equal(PetGender.Male, details.Gender);
        }

        [Fact]
        public void PetDetails_Should_Throw_When_PetType_Is_Invalid()
        {
            // Arrange
            var invalidPetType = "InvalidType";

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PetDetails(
                    "Title", "Description", 100, invalidPetType, "Dog", null, null, null,
                    "Male", false, null, null, null, "Good", false));
        }

        [Theory]
        [InlineData("Male", 1)]
        [InlineData("Female", 2)]
        public void PetGender_SmartEnum_Should_Work(string genderName, int expectedValue)
        {
            // Act
            var gender = PetGender.FromName(genderName, true);

            // Assert
            Assert.NotNull(gender);
            Assert.Equal(expectedValue, gender.Value);
        }

        [Fact]
        public void PetDetails_Should_Throw_When_AnimalType_Is_Null()
        {
            // Arrange
            string? animalType = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PetDetails(
                    "Title", "Description", 100, "Sell", animalType, null, null, null,
                    "Male", false, null, null, null, "Good", false));
        }

        [Fact]
        public void PetDetails_Should_Throw_When_Breed_Exceeds_Max_Length()
        {
            // Arrange
            var breed = new string('A', 101);

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PetDetails(
                    "Title", "Description", 100, "Sell", "Dog", null, breed, null,
                    "Male", false, null, null, null, "Good", false));
        }

        [Fact]
        public void PetDetails_Should_Throw_When_Price_Provided_For_Adoption()
        {
            // Arrange
            var petType = "Adoption";
            var price = 100;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PetDetails(
                    "Title", "Description", price, petType, "Dog", null, null, null,
                    "Male", false, null, null, "Adoption Requirements", "Good", false));
        }

        [Fact]
        public void PetDetails_Should_Throw_When_AdoptionRequirements_Not_Provided()
        {
            // Arrange
            var petType = "Adoption";
            string? adoptionRequirements = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PetDetails(
                    "Title", "Description", null, petType, "Dog", null, null, null,
                    "Male", false, null, null, adoptionRequirements, "Good", false));
        }

        [Fact]
        public void PetDetails_Should_Throw_When_AccessoryType_Not_Provided_For_Accessories()
        {
            // Arrange
            var petType = "Accessories";
            string? accessoryType = null;

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PetDetails(
                    "Title", "Description", null, petType, null, null, null, null,
                    null, false, accessoryType, new List<string> { "Leather" }, null, null, null));
        }

        [Fact]
        public void PetDetails_Should_Throw_When_Material_Exceeds_Max_Length_For_Accessories()
        {
            // Arrange
            var petType = "Accessories";
            var materials = new List<string> { new string('A', 51) };

            // Act & Assert
            Assert.Throws<DomainException>(() =>
                new PetDetails(
                    "Title", "Description", null, petType, null, null, null, null,
                    null, false, "Leash", materials, null, null, null));
        }
    }
}
