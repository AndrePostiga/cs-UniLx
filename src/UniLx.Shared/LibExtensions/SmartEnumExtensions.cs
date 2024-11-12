using Ardalis.SmartEnum;

namespace UniLx.Shared.LibExtensions
{
    public static class SmartEnumExtensions
    {
        public static bool HasSmartEnumValue<TEnum>(this TEnum value, TEnum targetValue)
            where TEnum : SmartEnum<TEnum, int>
        {
            return value == targetValue;
        }
    }
}
