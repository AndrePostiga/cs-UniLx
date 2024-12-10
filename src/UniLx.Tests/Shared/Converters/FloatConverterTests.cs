using System.Text.Json;
using UniLx.Shared.Converters;

namespace UniLx.Tests.Shared.Converters
{
    public class FloatConverterTests
    {
        [Fact]
        public void Read_ShouldDeserializeFloatCorrectly()
        {
            // Arrange
            var json = "3.14159";
            var options = new JsonSerializerOptions();
            options.Converters.Add(new FloatConverter(2));
            var reader = new Utf8JsonReader(System.Text.Encoding.UTF8.GetBytes(json));

            // Act
            reader.Read();
            var converter = new FloatConverter(2);
            var result = converter.Read(ref reader, typeof(float), options);

            // Assert
            Assert.Equal(3.14159f, result);
        }

        [Fact]
        public void Write_ShouldSerializeFloatWithSpecifiedDecimalPlaces()
        {
            // Arrange
            var options = new JsonSerializerOptions();
            options.Converters.Add(new FloatConverter(2));
            var value = 3.14159f;
            var converter = new FloatConverter(2);
            using var stream = new System.IO.MemoryStream();
            using var writer = new Utf8JsonWriter(stream);

            // Act
            converter.Write(writer, value, options);
            writer.Flush();
            var json = System.Text.Encoding.UTF8.GetString(stream.ToArray());

            // Assert
            Assert.Equal("3.14", json);
        }

        [Fact]
        public void Serialize_WithFloatConverter_ShouldRoundToSpecifiedDecimalPlaces()
        {
            // Arrange
            var options = new JsonSerializerOptions
            {
                Converters = { new FloatConverter(3) }
            };
            var value = 3.14159f;

            // Act
            var json = JsonSerializer.Serialize(value, options);

            // Assert
            Assert.Equal("3.142", json);
        }

        [Fact]
        public void Deserialize_WithFloatConverter_ShouldReturnExactValue()
        {
            // Arrange
            var options = new JsonSerializerOptions
            {
                Converters = { new FloatConverter(3) }
            };
            var json = "3.141";

            // Act
            var result = JsonSerializer.Deserialize<float>(json, options);

            // Assert
            Assert.Equal(3.141f, result);
        }
    }
}
