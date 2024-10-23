using Ardalis.SmartEnum.JsonNet;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Infra.Data.Database;
using UniLx.Infra.Data.Database.Options;
using UniLx.Infra.Data.Database.Repository;
using Weasel.Core;

namespace UniLx.Infra.Data.ServiceExtensions
{
    public static class DatabaseExtensions
    {
        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var databaseOptions = configuration.GetSection(DatabaseOptions.Section).Get<DatabaseOptions>();
            builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Section));


            builder.AddNpgsqlDataSource("postgresdb");
            //    , ops =>
            //{
            //    ops.ConnectionString = databaseOptions!.ConnectionString;
            //});

            builder.Services.AddMarten(opts =>
            {
                opts.Schema.Include<AccountRegistry>();
                opts.DatabaseSchemaName = "UniLxDb";
                opts.AutoCreateSchemaObjects = AutoCreate.All;

                opts.Schema.Include<AccountRegistry>();
                opts.Schema.Include<CategoryRegistry>();

                //opts.Policies.ForAllDocuments(x =>
                //{
                //    x.Metadata.CausationId.Enabled = true;
                //    x.Metadata.CorrelationId.Enabled = true;
                //    x.Metadata.Headers.Enabled = true;
                //});

                opts.UseNewtonsoftForSerialization(
                    casing: Casing.SnakeCase,
                    enumStorage: EnumStorage.AsString,
                    nonPublicMembersStorage: NonPublicMembersStorage.NonPublicDefaultConstructor | NonPublicMembersStorage.NonPublicSetters,
                    configure: (serializerOptions) =>
                    {
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<StorageType, int>());
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<AdvertisementType, int>());
                    }); 

            })
            .ApplyAllDatabaseChangesOnStartup()
            .UseLightweightSessions()
            .UseNpgsqlDataSource();

            builder.Services.AddSingleton<IMartenContext, MartenContext>();
            builder.Services.AddScoped<Domain.Data.IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<Domain.Data.IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<Domain.Data.ICategoryRepository, CategoryRepository>();

            return builder;
        }
    }

    public class AccountRegistry : MartenRegistry
    {
        public AccountRegistry()
        {
            For<Account>()
                .Identity(x => x.Id)
                .Duplicate(x => x.Cpf.Value, pgType: "varchar(20)", notNull: true)
                .Duplicate(x => x.Email.Value, pgType: "varchar(128)", notNull: true);

        //configure: idx =>
        //{
        //    idx.Name = "idx_email";
        //    idx.IsUnique = true;
        //}
        //configure: idx =>
        //{
        //    idx.Name = "idx_cpf";
        //    idx.Method = IndexMethod.hash;
        //    idx.IsUnique = true;
        //})
        }
    }

    public class CategoryRegistry : MartenRegistry
    {
        public CategoryRegistry()
        {
            For<Category>()
                .Index(x => new {x.Root, x.Id}, options =>
                {
                    options.IsUnique = true;
                    options.Name = "idx_root_name_category";
                })
                .Duplicate(x => x.Root.Name, pgType: "varchar(100)", notNull: true)
                .Duplicate(x => x.Name, pgType: "varchar(100)", notNull: true);
        }
    }
}
