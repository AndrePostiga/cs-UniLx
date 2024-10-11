using System.Runtime.CompilerServices;

namespace UniLx.Domain.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException() { }
        public DomainException(string? message) : base(message) { }
        public DomainException(string? message, Exception innerException) : base(message, innerException) { }

        public static void ThrowIf<T>(T argument, Func<T, bool> predicate, string? message = null, [CallerArgumentExpression("argument")] string? paramName = null)
        {
            if (predicate.Invoke(argument))
                throw new DomainException(message);
        }

        public static void ThrowIf(bool isInvalid, string? message = null)
        {
            if (isInvalid)
                throw new DomainException(message);
        }
    }
}
