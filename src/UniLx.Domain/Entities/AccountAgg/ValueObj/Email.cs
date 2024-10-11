using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AccountAgg.ValueObj
{
    public partial record Email
    {
        public string Value { get; }

        public Email(string value)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(value), "Email cannot be empty.");
            DomainException.ThrowIf(!IsValidEmail(value), "Invalid email format.");

            Value = value;
        }

        public override string ToString() => Value;

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase)]
        private static partial Regex EmailRegex();

        private static bool IsValidEmail(string email) => EmailRegex().IsMatch(email); 
    }
}
