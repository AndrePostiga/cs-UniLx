using System.Text.RegularExpressions;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.Seedwork.ValueObj
{
    public class Email
    {
        public string Value { get; private set; }

        private Email(){}

        // Define a static readonly Regex instance for email validation
        private static readonly Regex EmailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase | RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

        public Email(string value)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(value), "Email cannot be empty.");
            DomainException.ThrowIf(!IsValidEmail(value), "Invalid email format.");

            Value = value;
        }

        public override string ToString() => Value;

        private static bool IsValidEmail(string email) => EmailRegex.IsMatch(email);
    }
}
