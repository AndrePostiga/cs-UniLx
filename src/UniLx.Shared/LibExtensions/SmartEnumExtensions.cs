using Ardalis.SmartEnum;
using System.Diagnostics.CodeAnalysis;

namespace UniLx.Shared.LibExtensions
{
    [ExcludeFromCodeCoverage]
    public static class SmartEnumExtensions
    {
        public static bool HasSmartEnumValue<TEnum>(this TEnum value, TEnum targetValue)
            where TEnum : SmartEnum<TEnum, int>
        {
            return value == targetValue;
        }
    }
}
