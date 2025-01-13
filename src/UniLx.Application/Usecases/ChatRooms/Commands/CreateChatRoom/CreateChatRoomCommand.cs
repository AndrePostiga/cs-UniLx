using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom
{
    public class CreateChatRoomCommand : ICommand<IResult>
    {
        public string AccountId { get; private set; }
        public string AdvertisementId { get; private set; }

        public CreateChatRoomCommand(string? accountId, string? advertisementId)
        {
            AccountId = accountId ?? string.Empty;
            AdvertisementId = advertisementId ?? string.Empty;
        }
    }
    public class CreateChatRoomCommandValidator : AbstractValidator<CreateChatRoomCommand>
    {
        public CreateChatRoomCommandValidator()
        {
            RuleFor(x => x.AdvertisementId)
                .NotEmpty().WithMessage("AdvertisementId is required.")
                .MaximumLength(64).WithMessage("AdvertisementId must not exceed 64 characters.");

            RuleFor(x => x.AccountId)
                .NotEmpty().WithMessage("AccountId is required.")
                .MaximumLength(64).WithMessage("AccountId must not exceed 64 characters.");
        }
    }
}
