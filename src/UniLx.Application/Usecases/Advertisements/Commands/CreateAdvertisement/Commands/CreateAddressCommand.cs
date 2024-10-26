using FluentValidation;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Commands
{
    public class CreateAddressCommand
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string? Country { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
        public string? Neighborhood { get; set; }
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Complement { get; set; }

        public CreateAddressCommand(double? latitude = null, double? longitude = null,
                              string? country = null, string? state = null, string? city = null,
                              string? neighborhood = null, string? zipCode = null,
                              string? street = null, string? number = null, string? complement = null)
        {
            Latitude = latitude;
            Longitude = longitude;
            Country = country;
            State = state;
            City = city;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            Street = street;
            Number = number;
            Complement = complement;
        }

        public class CreateAddressCommandValidator : AbstractValidator<CreateAddressCommand>
        {
            public CreateAddressCommandValidator()
            {
                // Regra para garantir que sejam fornecidos coordenadas, endereço completo ou ambos
                RuleFor(x => x)
                    .Must(x =>
                        (x.Latitude.HasValue && x.Longitude.HasValue) || // Ou ambos Latitude e Longitude são fornecidos
                        (!string.IsNullOrEmpty(x.Country) && !string.IsNullOrEmpty(x.State) && // Ou todos os campos de endereço completo são fornecidos
                         !string.IsNullOrEmpty(x.City) && !string.IsNullOrEmpty(x.ZipCode))
                    )
                    .WithMessage("É necessário fornecer ambos latitude e longitude, um endereço completo ou ambos.");

                // Validação para coordenadas: se um é fornecido, ambos devem ser fornecidos
                RuleFor(x => x.Latitude)
                    .InclusiveBetween(-90, 90).WithMessage("A latitude deve estar entre -90 e 90.")
                    .When(x => x.Latitude.HasValue);

                RuleFor(x => x.Longitude)
                    .InclusiveBetween(-180, 180).WithMessage("A longitude deve estar entre -180 e 180.")
                    .When(x => x.Longitude.HasValue);

                RuleFor(x => x)
                    .Must(x => !(x.Latitude.HasValue ^ x.Longitude.HasValue))
                    .WithMessage("Ambos latitude e longitude devem ser fornecidos juntos se um deles for especificado.");

                // Validação para campos de endereço completo
                RuleFor(x => x.Country)
                    .NotEmpty().WithMessage("O país é obrigatório ao fornecer um endereço completo.")
                    .Length(2).WithMessage("O código do país deve ter exatamente 2 caracteres.")
                    .Matches("^[A-Za-z]+$").WithMessage("O código do país deve conter apenas letras.")
                    .When(x => !string.IsNullOrEmpty(x.Country) || !string.IsNullOrEmpty(x.State) ||
                               !string.IsNullOrEmpty(x.City) || !string.IsNullOrEmpty(x.ZipCode));

                RuleFor(x => x.State)
                    .NotEmpty().WithMessage("O estado é obrigatório ao fornecer um endereço completo.")
                    .Length(2).WithMessage("O código do estado deve ter exatamente 2 caracteres.")
                    .Matches("^[A-Za-z]+$").WithMessage("O código do estado deve conter apenas letras.")
                    .When(x => !string.IsNullOrEmpty(x.Country) || !string.IsNullOrEmpty(x.State) ||
                               !string.IsNullOrEmpty(x.City) || !string.IsNullOrEmpty(x.ZipCode));

                RuleFor(x => x.City)
                    .NotEmpty().WithMessage("A cidade é obrigatória ao fornecer um endereço completo.")
                    .When(x => !string.IsNullOrEmpty(x.Country) || !string.IsNullOrEmpty(x.State) ||
                               !string.IsNullOrEmpty(x.City) || !string.IsNullOrEmpty(x.ZipCode));

                RuleFor(x => x.ZipCode)
                    .NotEmpty().WithMessage("O CEP é obrigatório ao fornecer um endereço completo.")
                    .When(x => !string.IsNullOrEmpty(x.Country) || !string.IsNullOrEmpty(x.State) ||
                               !string.IsNullOrEmpty(x.City) || !string.IsNullOrEmpty(x.ZipCode));

                // Validação para campos opcionais com limite de tamanho máximo
                RuleFor(x => x.Neighborhood)
                    .MaximumLength(100).WithMessage("O bairro não deve exceder 100 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Neighborhood));

                RuleFor(x => x.Street)
                    .MaximumLength(100).WithMessage("A rua não deve exceder 100 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Street));

                RuleFor(x => x.Number)
                    .MaximumLength(50).WithMessage("O número não deve exceder 50 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Number));

                RuleFor(x => x.Complement)
                    .MaximumLength(64).WithMessage("O complemento não deve exceder 64 caracteres.")
                    .When(x => !string.IsNullOrEmpty(x.Complement));
            }
        }
    }
}
