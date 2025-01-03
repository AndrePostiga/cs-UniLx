using Microsoft.AspNetCore.Http;
using UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom.Mappers;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.ChatAgg;
using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.ChatRooms.Commands.CreateChatRoom
{
    internal class CreateChatRoomCommandHandler : ICommandHandler<CreateChatRoomCommand, IResult>
    {
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IRequestContext _requestContext;
        private readonly IAdvertisementRepository _advertisementRepository;

        public CreateChatRoomCommandHandler(
            IChatRoomRepository chatRoomRepository,
            IAccountRepository accountRepository,
            IRequestContext requestContext,
            IAdvertisementRepository advertisementRepository)
        {
            _chatRoomRepository = chatRoomRepository;
            _accountRepository = accountRepository;
            _requestContext = requestContext;
            _advertisementRepository = advertisementRepository;
        }

        public async Task<IResult> Handle(CreateChatRoomCommand request, CancellationToken cancellationToken)
        {
            if (request.AccountId != _requestContext.AccountId)
                return ChatRoomErrors.NotOwner.ToBadRequest();

            var account = await _accountRepository.FindOne(x => x.Id == request.AccountId, cancellationToken);
            if (account == null)
                return ChatRoomErrors.AccountNotFound.ToBadRequest();

            var alreadyCreatedChatRoom = await _chatRoomRepository.FindOne(
                x => x.AdvertisementId == request.AdvertisementId &&
                x.SenderId == request.AccountId,
                cancellationToken);

            if (alreadyCreatedChatRoom != null)
                return ChatRoomErrors.Conflict.ToBadRequest();

            Account advertisementOwner;
            var advertisement = await _advertisementRepository.FindOneWithInclude<Account>(
                x => x.Id == request.AdvertisementId,
                x => x.OwnerId,
                x => advertisementOwner = x,
                cancellationToken);

            if (advertisement == null)
                return ChatRoomErrors.AdvertisementNotFound.ToBadRequest();

            if (advertisement.IsExpired())
                return ChatRoomErrors.AdvertisementIsExpired.ToBadRequest();    

            if (advertisement.OwnerId == account.Id)
                return ChatRoomErrors.AdvertisementOwnerEqualToSenderId.ToBadRequest();

            var chatRoom = new ChatRoom(account, advertisement);

            _chatRoomRepository.InsertOne(chatRoom);
            _accountRepository.UpdateOne(account);
            await _chatRoomRepository.UnitOfWork.Commit(cancellationToken);
            return Results.Ok(chatRoom.ToResponse(advertisement));
        }
    }
}
