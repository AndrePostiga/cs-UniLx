using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture
{
    public class UpdateProfilePictureCommand : ICommand<IResult>
    {
        public string AccountId { get; set; }
        public string FileName { get; set; }

        public UpdateProfilePictureCommand(string? accountId, string? fileName)
        {
            AccountId = accountId ?? string.Empty;
            FileName = fileName ?? string.Empty;
        }
    }

    public class UpdateProfilePictureCommandValidator : AbstractValidator<UpdateProfilePictureCommand>
    {
        public UpdateProfilePictureCommandValidator()
        {
            RuleFor(x => x.AccountId)
                .NotEmpty()
                .WithMessage("Account ID is required.");

            RuleFor(x => x.FileName)
                .NotEmpty()
                .WithMessage("File name is required.")
                .Matches(@"^[a-zA-Z0-9_\-]+\.(jpg|jpeg|png|bmp)$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                .WithMessage("Invalid file name or extension. Supported extensions: .jpg, .jpeg, .png, .bmp.");
        }
    }
}
