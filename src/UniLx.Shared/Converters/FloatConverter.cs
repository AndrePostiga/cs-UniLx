using System.Text.Json;
using System.Text.Json.Serialization;

namespace UniLx.Shared.Converters
{
    public class FloatConverter : JsonConverter<float>
    {
        private readonly int _decimalPlaces;

        public FloatConverter(int decimalPlaces)
        {
            _decimalPlaces = decimalPlaces;
        }

        public override float Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return reader.GetSingle();
        }

        public override void Write(Utf8JsonWriter writer, float value, JsonSerializerOptions options)
        {
            var roundedValue = Math.Round(value, _decimalPlaces);
            writer.WriteNumberValue((float)roundedValue);
        }
    }
}
