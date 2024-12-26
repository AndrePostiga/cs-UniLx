using Microsoft.AspNetCore.SignalR;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.ChatAgg;

namespace UniLx.Infra.Services.ChatHubs
{
    public class AdvertisementMessageChatHub : Hub
    {
        private readonly IAdvertisementRepository _advertisementRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IChatRoomRepository _chatRoomRepository;
        private readonly IMessageRepository _messageRepository;

        public AdvertisementMessageChatHub(
            IAdvertisementRepository advertisementRepository,
            IAccountRepository accountRepository,
            IChatRoomRepository chatRoomRepository,
            IMessageRepository messageRepository)
        {
            _advertisementRepository = advertisementRepository;
            _accountRepository = accountRepository;
            _chatRoomRepository = chatRoomRepository;
            _messageRepository = messageRepository;
        }

        // Method to connect the user to the chat
        public async Task ConnectToChat(string chatRoomId, string userId)
        {
            // Verifica e o usuário está logado
            var existingChatRoom = await _chatRoomRepository.FindOne(x => x.Id == chatRoomId, CancellationToken.None);
            if (existingChatRoom is null)
            {
                await Clients.Caller.SendAsync("Error", "Chat room does not exist.");
                Context.Abort();
                return;
            }           

            await Groups.AddToGroupAsync(Context.ConnectionId, existingChatRoom.Id);            
            var (messages, count) = await _messageRepository.FindAll(
                skip: 1, 
                limit: 20, 
                sortAsc:true,
                x => x.RoomId == existingChatRoom.Id,                
                CancellationToken.None);

            await Clients.Caller.SendAsync("ReceiveHistory", messages?.Select(m => new
            {
                m.MessageOwnerId,
                m.MessageOwnerName,
                m.Content,
                m.SentAt,
                m.IsSeen
            }));
            
            await Clients.Group(existingChatRoom.Id).SendAsync("UserConnected", $"{userId} has joined the chat.");
        }
        
        public async Task SendMessageToChat(string chatRoomId, string content, string senderId)
        {
            var chatRoom = await _chatRoomRepository.FindOne
                (x => x.Id == chatRoomId,
                CancellationToken.None);

            var senderAccount = await _accountRepository.FindOne(x => x.Id == senderId, CancellationToken.None);

            if (chatRoom is null || senderAccount is null)
            {
                await Clients.Caller.SendAsync("Error", "Chat room or sender account does not exist.");
                Context.Abort();
                return;
            }

            if (chatRoom is null)
            {
                await Clients.Caller.SendAsync("Error", "Chat room not found.");
                return;
            }

            var message = new Message(content, chatRoom, senderAccount);            
            _messageRepository.InsertOne(message);
            
            await Clients.Group(chatRoom.Id).SendAsync("ReceiveMessage", new
            {
                message.MessageOwnerId,
                message.MessageOwnerName,
                message.Content,
                message.SentAt,
                message.IsSeen
            });

            await _messageRepository.UnitOfWork.Commit();
        }
        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            await base.OnDisconnectedAsync(exception);
        }
    }
}
