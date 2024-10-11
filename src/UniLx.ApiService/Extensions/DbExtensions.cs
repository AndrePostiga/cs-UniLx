using Marten;
using UniLx.Domain.Entities.AccountAgg;

namespace UniLx.ApiService.Extensions
{
    public static class DbExtensions
    {
        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            builder.AddNpgsqlDataSource("postgresdb");

            builder.Services.AddMarten(opts =>
            {
                opts.Schema.Include<AccountRegistry>();
                opts.DatabaseSchemaName = "UniLxDb";
                opts.AutoCreateSchemaObjects = Weasel.Core.AutoCreate.All;

            })
            .ApplyAllDatabaseChangesOnStartup()
            .UseLightweightSessions()
            .UseNpgsqlDataSource();

            return builder;
        }
    }

    public class AccountRegistry : MartenRegistry
    {
        public AccountRegistry()
        {
            For<Account>()
                .Identity(x => x.Id)
                .Duplicate(x => x.Cpf.Value)
                .Duplicate(x => x.Email.Value);
        }
    }
}
