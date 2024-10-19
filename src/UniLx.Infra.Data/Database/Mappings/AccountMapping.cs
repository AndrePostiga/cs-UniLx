using Marten;
using UniLx.Domain.Entities.AccountAgg;

namespace UniLx.Infra.Data.Database.Mappings
{
    public class AccountMapping : MartenRegistry
    {
        public AccountMapping()
        {
            For<Account>()
                .Identity(x => x.Id)
                .Duplicate(x => x.Cpf.Value)
                .Duplicate(x => x.Email.Value);
        }
    }
}
