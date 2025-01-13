using System.Linq.Expressions;
using UniLx.Domain.Entities.ChatAgg;

namespace UniLx.Domain.Data
{
    public interface IMessageRepository : IRepository<Message>
    {
        Task<IEnumerable<Message>> GetMessagesForRoom(string roomId, CancellationToken ct);
        Task SaveMessageAsync(Message message, CancellationToken cancellationToken);
    }
}
