using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using System.Linq.Expressions;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands.UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.DetailsCommands;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails;
using UniLx.Domain.Entities.Seedwork;
using UniLx.Domain.Services;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;
using UniLx.Shared.Abstractions;
using UniLx.Tests.Domain.AdvertisementAgg;
using UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Factories;
using Ardalis.SmartEnum;

namespace UniLx.Tests.Application.UseCases.CreateAdvertisement
{
    public class CreateAdvertisementHandlerTests
    {
        private readonly Mock<IAccountRepository> _accountRepositoryMock;
        private readonly Mock<ICategoryRepository> _categoryRepositoryMock;
        private readonly Mock<ICreateAdvertisementDomainService> _createAdvertisementDomainServiceMock;
        private readonly Mock<IStorageRepository<AccountBucketOptions>> _accountStorageMock;
        private readonly CreateAdvertisementCommandHandler _handler;
        private readonly Fixture _autoFixture;

        public CreateAdvertisementHandlerTests()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _categoryRepositoryMock = new Mock<ICategoryRepository>();
            _createAdvertisementDomainServiceMock = new Mock<ICreateAdvertisementDomainService>();
            _accountStorageMock = new Mock<IStorageRepository<AccountBucketOptions>>();
            _autoFixture = new Fixture();
            _autoFixture.Customize<Category>(c => c.FromFactory(() 
                => Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos", "Residential rentals")));

            _autoFixture.Customizations.Add(new TypeRelay(
                typeof(Details),
                typeof(RealEstateDetails)
            ));

            _autoFixture.Customize<RealEstateDetails>(c => c
                .FromFactory(() => new RealEstateDetails(
                    title: "Valid Title",
                    description: "Valid Description",
                    price: 100000,
                    lotSizeInSquareMeters: 500,
                    propertyType: "residential", // Valid property type
                    condition: "new",
                    constructedSquareFootage: 250,
                    bedrooms: 3,
                    bathrooms: 2,
                    parkingSpaces: 1,
                    floors: 2,
                    additionalFeatures: new List<string> { "Pool", "Garage" }
                )));

            _autoFixture.Customize<Address>(c => c
    .FromFactory(() => Address.CreateAddress(
        latitude: 25.0, // Valid latitude
        longitude: 45.0, // Valid longitude
        country: "BR",
        state: "RJ",
        city: "Rio de Janeiro",
        neighborhood: "Downtown",
        zipCode: "12345",
        street: "Main St",
        number: "123",
        complement: "Apt 4"
    )));

            _autoFixture.Customize<Account>(c => c
    .FromFactory(() => new Account(
        name: "Test User",
        email: "test@example.com", // Valid email format
        Cpf: "12345678909", // Valid CPF
        description: "A test account",
        cognitoSubscriptionId: Guid.NewGuid().ToString()
    )));

            _autoFixture.Customize<Advertisement>(c => c
    .FromFactory(() => new Advertisement(
        type: "real_estate",
        subCategory: Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos", "Residential rentals"),
        details: new TestDetailsStub("Valid Title", "Valid Description", 100),
        expiresAt: DateTime.UtcNow.AddDays(30), // Valid expiration date
        address: Address.CreateAddress(
            latitude: 25.0,
            longitude: 45.0,
            country: "BR",
            state: "RJ",
            city: "Rio de Janeiro",
            neighborhood: "Downtown",
            zipCode: "12345",
            street: "Main St",
            number: "123",
            complement: "Apt 4"
        ),
        account: new Account(
            name: "Test User",
            email: "test@example.com",
            Cpf: "12345678909",
            description: "A test account",
            cognitoSubscriptionId: Guid.NewGuid().ToString()
        )
    )));


            _handler = new CreateAdvertisementCommandHandler(
                _accountRepositoryMock.Object,
                _accountStorageMock.Object,
                _categoryRepositoryMock.Object,
                _createAdvertisementDomainServiceMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequest_When_Account_Not_Found()
        {
            // Arrange
            var command = new CreateAdvertisementCommand(
                accountId: "nonexistent-account",
                type: "real_estate",
                subCategory: "Rentals",
                expiresAt: DateTime.UtcNow.AddDays(30),
                address: new CreateAddressCommand(
                    latitude: null,
                    longitude: null,
                    country: "BR",
                    state: "RJ",
                    city: "Rio de Janeiro",
                    neighborhood: null,
                    zipCode: "12345",
                    street: null,
                    number: null,
                    complement: null
                ),
                beautyDetails: null,
                eventDetails: null,
                electronicsDetails: null,
                fashionDetails: null,
                jobOpportunitiesDetails: null,
                petDetails: null,
                realStateDetails: new CreateRealEstateDetailsCommand(
                    title: "Valid Title",
                    description: "Valid Description",
                    price: 100,
                    lotSizeInSquareMeters: 500,
                    constructedSquareFootage: 250,
                    bedrooms: 3,
                    bathrooms: 2,
                    parkingSpaces: 1,
                    propertyType: "Residential",
                    condition: "New",
                    floors: 2,
                    additionalFeatures: new List<string> { "Pool", "Garage" }
                ),
                othersDetails: null);

            _accountRepositoryMock.Setup(repo => repo.FindOne(It.IsAny<Expression<Func<Account, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(Account));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None) as Microsoft.AspNetCore.Http.IStatusCodeHttpResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequest_When_Category_Not_Found()
        {
            // Arrange
            var account = new Account("Test User", "test@example.com", "24313678352", "Description", Guid.NewGuid().ToString());
            var command = new CreateAdvertisementCommand(
                accountId: account.Id,
                type: "real_estate",
                subCategory: "Invalid Subcategory",
                expiresAt: DateTime.UtcNow.AddDays(30),
                address: new CreateAddressCommand(
                    latitude: null,
                    longitude: null,
                    country: "BR",
                    state: "RJ",
                    city: "Rio de Janeiro",
                    neighborhood: null,
                    zipCode: "12345",
                    street: null,
                    number: null,
                    complement: null
                ),
                beautyDetails: null,
                eventDetails: null,
                electronicsDetails: null,
                fashionDetails: null,
                jobOpportunitiesDetails: null,
                petDetails: null,
                realStateDetails: new CreateRealEstateDetailsCommand(
                    title: "Valid Title",
                    description: "Valid Description",
                    price: 100,
                    lotSizeInSquareMeters: 500,
                    constructedSquareFootage: 250,
                    bedrooms: 3,
                    bathrooms: 2,
                    parkingSpaces: 1,
                    propertyType: "Residential",
                    condition: "New",
                    floors: 2,
                    additionalFeatures: new List<string> { "Pool", "Garage" }
                ),
                othersDetails: null);

            _accountRepositoryMock.Setup(repo => repo.FindOne(It.IsAny<Expression<Func<Account, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            _categoryRepositoryMock.Setup(repo => repo.FindOne(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(Category));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None) as Microsoft.AspNetCore.Http.IStatusCodeHttpResult; ;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task Handle_Should_Return_BadRequest_When_Advertisement_Creation_Fails()
        {
            // Arrange
            var account = new Account("Test User", "test@example.com", "24313678352", "Description", Guid.NewGuid().ToString());
            var category = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos", "Residential rentals");
            var command = new CreateAdvertisementCommand(
                accountId: account.Id,
                type: "real_estate",
                subCategory: "Rentals",
                expiresAt: DateTime.UtcNow.AddDays(30),
                address: new CreateAddressCommand(
                    latitude: null,
                    longitude: null,
                    country: "BR",
                    state: "RJ",
                    city: "Rio de Janeiro",
                    neighborhood: null,
                    zipCode: "12345",
                    street: null,
                    number: null,
                    complement: null
                ),
                beautyDetails: null,
                eventDetails: null,
                electronicsDetails: null,
                fashionDetails: null,
                jobOpportunitiesDetails: null,
                petDetails: null,
                realStateDetails: new CreateRealEstateDetailsCommand(
                    title: "Valid Title",
                    description: "Valid Description",
                    price: 100,
                    lotSizeInSquareMeters: 500,
                    constructedSquareFootage: 250,
                    bedrooms: 3,
                    bathrooms: 2,
                    parkingSpaces: 1,
                    propertyType: "Residential",
                    condition: "New",
                    floors: 2,
                    additionalFeatures: new List<string> { "Pool", "Garage" }
                ),
                othersDetails: null);

            _accountRepositoryMock.Setup(repo => repo.FindOne(It.IsAny<Expression<Func<Account, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            _categoryRepositoryMock.Setup(repo => repo.FindOne(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            _createAdvertisementDomainServiceMock.Setup(service => service.CreateAdvertisement(
                It.IsAny<AdvertisementType>(),
                It.IsAny<Category>(),
                It.IsAny<Details>(),
                It.IsAny<DateTime?>(),
                It.IsAny<Account>(),
                It.IsAny<Address>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResult<Advertisement>.Failure(Error.BadRequest));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None) as Microsoft.AspNetCore.Http.IStatusCodeHttpResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public async Task Handle_Should_Return_Ok_When_Advertisement_Is_Created()
        {
            // Arrange
            var account = new Account("Test User", "test@example.com", "24313678352", "Description", Guid.NewGuid().ToString());
            var category = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos", "Residential rentals");

            var command = new CreateAdvertisementCommand(
                accountId: account.Id,
                type: "real_estate",
                subCategory: "Rentals",
                expiresAt: DateTime.UtcNow.AddDays(30),
                address: new CreateAddressCommand(
                    latitude: null,
                    longitude: null,
                    country: "BR",
                    state: "RJ",
                    city: "Rio de Janeiro",
                    neighborhood: null,
                    zipCode: "12345",
                    street: null,
                    number: null,
                    complement: null
                ),
                beautyDetails: null,
                eventDetails: null,
                electronicsDetails: null,
                fashionDetails: null,
                jobOpportunitiesDetails: null,
                petDetails: null,
                realStateDetails: new CreateRealEstateDetailsCommand(
                    title: "Valid Title",
                    description: "Valid Description",
                    price: 100,
                    lotSizeInSquareMeters: 500,
                    constructedSquareFootage: 250,
                    bedrooms: 3,
                    bathrooms: 2,
                    parkingSpaces: 1,
                    propertyType: "Residential",
                    condition: "New",
                    floors: 2,
                    additionalFeatures: new List<string> { "Pool", "Garage" }
                ),
                othersDetails: null);

            _accountRepositoryMock.Setup(repo => repo.FindOne(It.IsAny<Expression<Func<Account, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            _categoryRepositoryMock.Setup(repo => repo.FindOne(It.IsAny<Expression<Func<Category, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            var fakeAccount = new Account("Test User", "test@example.com", "24313678352", "A test account", Guid.NewGuid().ToString());
            var fakeAddress = Address.CreateAddress(25.0, 45.0, "BR", "RJ", "Rio de Janeiro", "Downtown", "12345", "Main St", "123", "Apt 4");
            var fakeCategory = Category.CreateNewCategory("real_estate", "Rentals", "Apartamentos", "Residential rentals");
            var fakeAdvertisement = new Advertisement(AdvertisementType.RealEstate.Name,
                fakeCategory,
                command.ToDetails(),
                null,
                fakeAddress,
                fakeAccount);

            _createAdvertisementDomainServiceMock.Setup(service => service.CreateAdvertisement(
                It.IsAny<AdvertisementType>(),
                It.IsAny<Category>(),
                It.IsAny<Details>(),
                It.IsAny<DateTime?>(),
                It.IsAny<Account>(),
                It.IsAny<Address>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(ServiceResult<Advertisement>.Success(fakeAdvertisement));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None) as Microsoft.AspNetCore.Http.IStatusCodeHttpResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Handle_Should_Validate_Advertisement_Type()
        {
            // Arrange
            var account = new Account("Test User", "test@example.com", "24313678352", "Description", Guid.NewGuid().ToString());
            var command = new CreateAdvertisementCommand(
                accountId: account.Id,
                type: "invalid_type",
                subCategory: "Rentals",
                expiresAt: DateTime.UtcNow.AddDays(30),
                address: new CreateAddressCommand(
                        latitude: null,
                        longitude: null,
                        country: "BR",
                        state: "RJ",
                        city: "Rio de Janeiro",
                        neighborhood: null,
                        zipCode: "12345",
                        street: null,
                        number: null,
                        complement: null
                    ),
                beautyDetails: null,
                eventDetails: null,
                electronicsDetails: null,
                fashionDetails: null,
                jobOpportunitiesDetails: null,
                petDetails: null,
                realStateDetails: null,
                othersDetails: null);

            _accountRepositoryMock.Setup(repo => repo.FindOne(It.IsAny<Expression<Func<Account, bool>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(account);

            // Act & Assert
            await Assert.ThrowsAsync<SmartEnumNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
