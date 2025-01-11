using UniLx.Domain.Data;
using UniLx.Domain.Entities.ChatAgg;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(IMartenContext martenContext, IUnitOfWork unitOfWork) : base(martenContext, unitOfWork)
        {
        }

        public async Task<IEnumerable<Message>> GetMessagesForRoom(string roomId, CancellationToken ct)
        {
            var result = await FindAll(0, 100, true, x => x.RoomId == roomId, ct);
            return result.Item1 ?? [];
        }

        // Save a new message
        public async Task SaveMessageAsync(Message message, CancellationToken cancellationToken)
        {
            InsertOne(message);
            await UnitOfWork.Commit(cancellationToken);
        }
    }
}
