using FluentValidation;
using Microsoft.AspNetCore.Http;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.ChatRooms.Queries.GetChatRooms.User
{
    public class GetUserChatRoomsQuery : IQuery<IResult>
    {
        public string SenderId { get; private set; }
        public int Page { get; private set; }
        public int PageSize { get; private set; }

        public GetUserChatRoomsQuery(string? senderId, int? page, int? pageSize)
        {
            SenderId = senderId ?? string.Empty;
            Page = page ?? 1;
            PageSize = pageSize ?? 10;
        }
    }

    public class GetUserChatRoomsQueryValidator : AbstractValidator<GetUserChatRoomsQuery>
    {
        public GetUserChatRoomsQueryValidator()
        {
            RuleFor(x => x.SenderId)
                .NotEmpty().WithMessage("SenderId is required.")
                .MaximumLength(64).WithMessage("SenderId must not exceed 64 characters.");

            RuleFor(query => query.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page must be at least 1.");

            RuleFor(query => query.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("PageSize must not exceed 20.");


        }
    }
}
