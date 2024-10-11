namespace UniLx.Shared
{
    public static partial class ExtensionsHelper
    {
        public static string OnlyNumbers(this string source) => string.IsNullOrWhiteSpace(source) ? source : new(source.Where(c => char.IsDigit(c)).ToArray());

        public static string OnlyLetters(this string source) => string.IsNullOrWhiteSpace(source) ? source : new(source.Where(c => char.IsLetter(c)).ToArray());

        public static string OnlyLetterOrNumbers(this string source) => string.IsNullOrWhiteSpace(source) ? source : new(source.Where(c => char.IsLetterOrDigit(c)).ToArray());

        public static string OnlyLetterOrNumbersWithSpaces(this string source) => string.IsNullOrWhiteSpace(source) ? source : new(source.Where(c => char.IsLetterOrDigit(c) || char.IsWhiteSpace(c)).ToArray());

        public static string WithoutSpaces(this string source) => string.IsNullOrWhiteSpace(source) ? source : new(source.Where(s => !char.IsWhiteSpace(s)).ToArray());
        
        public static string ToTitleCase(this string source) => string.Concat(source.Substring(0, 1).ToUpper(), source.Substring(1));

        public static string WithTrimmedSpaces(this string source) => string.IsNullOrWhiteSpace(source) ? source : string.Join(" ", source!.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));

        public static List<string> WithTrimmedSpaces(this List<string> source)
        {
            for (var i = 0; i < source.Count; i++)
                source[i] = source[i].WithTrimmedSpaces();

            return source;
        }
    }
}
