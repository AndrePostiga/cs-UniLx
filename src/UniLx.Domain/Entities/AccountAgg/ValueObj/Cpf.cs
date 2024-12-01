using System;
using System.Text.RegularExpressions;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AccountAgg.ValueObj
{
    public class CPF
    {
        private const int CpfLength = 11;
        public string Value { get; private set; }

        private static readonly Regex CpfSymbolsRegex = new Regex("[^0-9]", RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

        private CPF() { }

        public CPF(string value)
        {
            DomainException.ThrowIf(!IsValid(value), "Invalid CPF.");
            Value = value;
        }

        public static bool IsValid(string cpf)
        {
            // Remove non-numeric characters
            cpf = CpfSymbolsRegex.Replace(cpf, string.Empty);

            if (cpf.Length != CpfLength)
                return false;

            // Check if all digits are the same (invalid CPF)
            if (new string(cpf[0], CpfLength) == cpf)
                return false;

            // Validate first digit
            if (!ValidateCpfDigit(cpf, 9))
                return false;

            // Validate second digit
            return ValidateCpfDigit(cpf, 10);
        }

        private static bool ValidateCpfDigit(string cpf, int position)
        {
            int sum = 0;
            int weight = position + 1;

            for (int i = 0; i < position; i++)
            {
                sum += (cpf[i] - '0') * weight--;
            }

            int remainder = sum % 11;
            int digit = remainder < 2 ? 0 : 11 - remainder;

            return cpf[position] - '0' == digit;
        }

        public override string ToString() => Value;
    }
}
