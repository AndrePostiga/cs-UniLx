using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements.Commands.AdvertisementGeneratePresignUrl
{
    public class AdvertisementGeneratePresignUrlCommand : ICommand<IResult>
    {
        public string AdvertisementId { get; set; }
        public string FileName { get; set; }
        public string AccountId { get; set; }

        public AdvertisementGeneratePresignUrlCommand(string? advertisementId, string? fileName, string? accountId)
        {
            AdvertisementId = advertisementId ?? string.Empty;
            FileName = fileName ?? string.Empty;
            AccountId = accountId ?? string.Empty;
        }
    }

    public class AdvertisementGeneratePresignUrlCommandValidator : AbstractValidator<AdvertisementGeneratePresignUrlCommand>
    {
        public AdvertisementGeneratePresignUrlCommandValidator()
        {
            RuleFor(x => x.AdvertisementId)
                .NotEmpty().WithMessage("AdvertisementId is required.")
                .MaximumLength(64).WithMessage("AdvertisementId must not exceed 50 characters.");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("FileName is required.")
                .Matches(@"^[\w,\s-]+\.[A-Za-z]{3,4}$").WithMessage("Invalid FileName format.")
                .MaximumLength(100).WithMessage("FileName must not exceed 100 characters.");

            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("AccountId is required.")
                .MaximumLength(64).WithMessage("AccountId must not exceed 50 characters.");
        }
    }
}
