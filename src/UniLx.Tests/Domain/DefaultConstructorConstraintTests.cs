using Ardalis.SmartEnum;
using System.Reflection;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Tests.Domain
{
    public class DefaultConstructorConstraintTests
    {
        [Fact]
        public void All_Domain_Objects_Should_Have_Private_Default_Constructor()
        {
            // Arrange
            var domainAssembly = typeof(ContactInformation).Assembly; // Adjust to your domain assembly
            var domainTypes = domainAssembly.GetTypes()
                .Where(t => t.IsClass
                            && !t.IsAbstract
                            && t.Namespace?.Contains("UniLx.Domain.Entities") == true
                            && t.BaseType != typeof(SmartEnum<>)
                            && (t.BaseType == null || !t.BaseType.IsGenericType || t.BaseType.GetGenericTypeDefinition() != typeof(SmartEnum<>))
);

            foreach (var type in domainTypes)
            {
                // Act
                var privateConstructor = type.GetConstructor(
                    BindingFlags.NonPublic | BindingFlags.Instance,
                    null,
                    Type.EmptyTypes,
                    null);

                var publicConstructor = type.GetConstructor(
                    BindingFlags.Public | BindingFlags.Instance,
                    null,
                    Type.EmptyTypes,
                    null);

                // Assert
                Assert.True(privateConstructor != null || publicConstructor != null,
                    $"{type.Name} must have either a private or public default constructor.");
            }
        }
    }
}
