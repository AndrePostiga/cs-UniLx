using System;
using System.Text.RegularExpressions;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AccountAgg.ValueObj
{
    public partial record CPF
    {
        private const int CpfLength = 11;
        public string Value { get; }

        public CPF(string value)
        {
            DomainException.ThrowIf(!IsValid(value), "Invalid CPF.");
            Value = value;
        }

        private static bool IsValid(string cpf)
        {
            // Remove non-numeric characters
            cpf = CpfSymbolsRegex().Replace(cpf, string.Empty);

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

        [GeneratedRegex("[^0-9]")]
        private static partial Regex CpfSymbolsRegex();
    }
}
