using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.Seedwork.ValueObj
{
    public class Phone
    {
        public string CountryCode { get; private set; }
        public string AreaCode { get; private set; }
        public string Number { get; private set; }

        private static readonly HashSet<string> CountryCodes = new() { "55" };

        private static readonly Dictionary<string, HashSet<string>> AreaCodes = new()
        {
            { "Centro-Oeste", new HashSet<string> { "61", "62", "64", "65", "66", "67" } },
            { "Nordeste", new HashSet<string> { "82", "71", "73", "74", "75", "77", "85", "88", "98", "99", "83", "81", "87", "86", "89", "84", "79" } },
            { "Norte", new HashSet<string> { "68", "96", "92", "97", "91", "93", "94", "69", "95", "63" } },
            { "Sudeste", new HashSet<string> { "27", "28", "31", "32", "33", "34", "35", "37", "38", "21", "22", "24", "11", "12", "13", "14", "15", "16", "17", "18", "19" } },
            { "Sul", new HashSet<string> { "41", "42", "43", "44", "45", "46", "51", "53", "54", "55", "47", "48", "49" } }
        };

        // accept only "9XXXXXXXX" or "XXXXXXXX"
        private static readonly Regex NumberRegex = new(@"^(9\d{8}|\d{8})$", RegexOptions.Compiled);

        private Phone() { }

        public Phone(string countryCode, string areaCode, string number)
        {
            SetCountryCode(countryCode);
            SetAreaCode(areaCode);
            SetNumber(number);
        }

        private void SetCountryCode(string countryCode)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(countryCode), "Country code cannot be empty.");
            DomainException.ThrowIf(!CountryCodes.Contains(countryCode), "Invalid country code. Expected one of: " + string.Join(", ", CountryCodes.Select(c => "+" + c)));
            CountryCode = countryCode;
        }

        private void SetAreaCode(string areaCode)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(areaCode), "Area code cannot be empty.");
            DomainException.ThrowIf(!AreaCodes.Values.Any(regionCodes => regionCodes.Contains(areaCode)), "Invalid area code.");
            AreaCode = areaCode;
        }

        private void SetNumber(string number)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(number), "Phone number cannot be empty.");
            DomainException.ThrowIf(!NumberRegex.IsMatch(number), "Invalid phone number format. Expected format: 9XXXXXXXX or XXXXXXXX.");
            Number = number;
        }

        public override string ToString() => $"+{CountryCode} ({AreaCode}) {Number}";
    }
}
