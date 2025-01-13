using UniLx.Domain.Data;
using UniLx.Domain.Entities.ChatAgg;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class ChatRoomRepository : Repository<ChatRoom>, IChatRoomRepository
    {
        public ChatRoomRepository(IMartenContext martenContext, IUnitOfWork unitOfWork) : base(martenContext, unitOfWork)
        {
        }
    }
}
