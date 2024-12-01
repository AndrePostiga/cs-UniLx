using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.Seedwork;
using UniLx.Domain.Exceptions;
using UniLx.Tests.Domain.AdvertisementAgg;

namespace UniLx.Tests.Domain.AccountAgg
{

    public class AccountTests
    {
        #region Constructor Tests

        [Fact]
        public void Constructor_ValidInput_ShouldInitializeCorrectly()
        {
            // Arrange
            string name = "John Doe";
            string email = "john.doe@example.com";
            string cpf = "93541134780";
            string description = "Test description";

            // Act
            var account = new Account(name, email, cpf, description);

            // Assert
            Assert.Equal(name, account.Name);
            Assert.Equal(email, account.Email.Value);
            Assert.Equal(cpf, account.Cpf.Value);
            Assert.Equal("Test description", account.Description);
        }

        [Fact]
        public void Constructor_NullName_ShouldThrowException()
        {
            // Arrange
            string email = "john.doe@example.com";
            string cpf = "93541134780";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Account(null, email, cpf, null));
            Assert.Equal("Name cannot be null.", ex.Message);
        }

        [Fact]
        public void Constructor_EmptyName_ShouldThrowException()
        {
            // Arrange
            string email = "john.doe@example.com";
            string cpf = "93541134780";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Account("", email, cpf, null));
            Assert.Equal("Name cannot be null.", ex.Message);
        }

        [Fact]
        public void Constructor_NameExceedsMaxLength_ShouldThrowException()
        {
            // Arrange
            string longName = new string('A', 101);
            string email = "john.doe@example.com";
            string cpf = "93541134780";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Account(longName, email, cpf, null));
            Assert.Equal("Name field must have 100 characters or less", ex.Message);
        }

        [Fact]
        public void Constructor_InvalidCpf_ShouldThrowException()
        {
            // Arrange
            string name = "John Doe";
            string email = "john.doe@example.com";
            string invalidCpf = "123456789";

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Account(name, email, invalidCpf, null));
            Assert.Equal("Invalid CPF.", ex.Message);
        }

        #endregion

        #region UpdateProfilePicture Tests

        [Fact]
        public void UpdateProfilePicture_ValidInput_ShouldSetProfilePicture()
        {
            // Arrange
            var account = new Account("John Doe", "john.doe@example.com", "93541134780", null);
            string profilePicturePath = "profile_picture.jpg";
            string expectedPath = $"{account.Id}/{profilePicturePath}";

            // Act
            account.UpdateProfilePicture(profilePicturePath);

            // Assert
            Assert.NotNull(account.ProfilePicture);
            
            Assert.Equal(expectedPath, account.ProfilePicture!.FullPath);
        }

        [Fact]
        public void UpdateProfilePicture_NullOrWhitespace_ShouldNotSetProfilePicture()
        {
            // Arrange
            var account = new Account("John Doe", "john.doe@example.com", "93541134780", null);

            // Act
            account.UpdateProfilePicture(null);

            // Assert
            Assert.Null(account.ProfilePicture);
        }

        #endregion

        #region AddAdvertisement Tests

        [Fact]
        public void AddAdvertisement_ValidAdvertisement_ShouldAddToAdvertisementIds()
        {
            // Arrange
            var account = new Account("John Doe", "john.doe@example.com", "93541134780", null);

            // Arrange
            var category = Category.CreateNewCategory("services", "HomeCleaning", "Limpeza Doméstica", "Residential cleaning services.");
            var address = Address.CreateAddress(country: "BR", state: "RJ", city: "Rio de Janeiro", zipCode: "12345");            
            var details = new TestDetailsStub("Valid Title", "Valid Description", 100);

            // Act
            var advertisement = new Advertisement(
                "services",
                category,
                details,
                DateTime.UtcNow.AddDays(30),
                address,
                account);

            // Act
            account.AddAdvertisement(advertisement);

            // Assert
            Assert.Single(account.AdvertisementIds!);

            account.AdvertisementIds!.TryGetValue(advertisement.Id, out string? id);
            Assert.Equal(advertisement.Id, id);
        }


        [Fact]
        public void AddAdvertisement_NullAdvertisement_ShouldThrowException()
        {
            // Arrange
            var account = new Account("John Doe", "john.doe@example.com", "93541134780", null);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => account.AddAdvertisement(null));
            Assert.Equal("Advertisement cannot be null.", ex.Message);
        }

        #endregion

        #region Description Tests

        [Fact]
        public void Constructor_DescriptionExceedsMaxLength_ShouldThrowException()
        {
            // Arrange
            string longDescription = new string('A', 257);

            // Act & Assert
            var ex = Assert.Throws<DomainException>(() => new Account("John Doe", "john.doe@example.com", "93541134780", longDescription));
            Assert.Equal("Description field must have 256 characters or less", ex.Message);
        }

        [Fact]
        public void Constructor_ValidDescription_ShouldEncodeHtml()
        {
            // Arrange
            string description = "<b>Bold description</b>";

            // Act
            var account = new Account("John Doe", "john.doe@example.com", "93541134780", description);

            // Assert
            Assert.Equal("&lt;b&gt;Bold description&lt;/b&gt;", account.Description);
        }

        #endregion
    }

}
