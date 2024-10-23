using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Accounts.Commands.UpdateProfilePicture
{
    public class UpdateProfilePictureCommand : ICommand<IResult>
    {
        public string AccountId { get; set; }

        public string? ProfilePicture { get; set; }

        public UpdateProfilePictureCommand(string? profilePicturePath, string accountId)
        {
            ProfilePicture = profilePicturePath;
            AccountId = accountId;
        }
    }

    public class UpdateProfilePictureCommandValidator : AbstractValidator<UpdateProfilePictureCommand>
    {
        public UpdateProfilePictureCommandValidator()
        {
            RuleFor(x => x.ProfilePicture)
                .NotEmpty()
                .WithMessage("ProfilePicturePath is required.")
                .Must(StorageImage.ValidateImageFileName);
        }
    }
}
