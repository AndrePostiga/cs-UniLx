using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class AccountRepository : Repository<Account>, IAccountRepository
    {
        public AccountRepository(IMartenContext martenContext, IUnitOfWork unitOfWork) : base(martenContext, unitOfWork)
        {
        }
    }
}
