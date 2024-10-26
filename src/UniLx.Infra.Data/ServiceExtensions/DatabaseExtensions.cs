using Ardalis.SmartEnum;
using Ardalis.SmartEnum.JsonNet;
using Marten;
using Marten.Linq.Members;
using Marten.Linq.Parsing;
using Marten.Schema;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq.Expressions;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Infra.Data.Database;
using UniLx.Infra.Data.Database.Options;
using UniLx.Infra.Data.Database.Repository;
using Weasel.Core;
using Weasel.Postgresql.SqlGeneration;

namespace UniLx.Infra.Data.ServiceExtensions
{
    public static class DatabaseExtensions
    {
        public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
        {
            var configuration = builder.Configuration;
            var databaseOptions = configuration.GetSection(DatabaseOptions.Section).Get<DatabaseOptions>();
            builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.Section));


            builder.AddNpgsqlDataSource("asdsd"
                , ops =>
            {
                ops.ConnectionString = databaseOptions!.ConnectionString;
            });

            builder.Services.AddMarten(opts =>
            {
                opts.Schema.Include<AccountRegistry>();
                opts.DatabaseSchemaName = "UniLxDb";
                opts.Linq.MethodCallParsers.Add(new HasSmartEnumValueParser<AdvertisementStatus>());
                opts.AutoCreateSchemaObjects = AutoCreate.All;                

                opts.Schema.Include<AccountRegistry>();
                opts.Schema.Include<CategoryRegistry>();
                opts.Schema.Include<AdvertisementRegistry>();

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
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<AdvertisementStatus, int>());                        
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<AddressType, int>());                        
                    });       
            })
            .InitializeWith(new SeedData())
            .ApplyAllDatabaseChangesOnStartup()
            .UseLightweightSessions()
            .UseNpgsqlDataSource();

            builder.Services.AddSingleton<IMartenContext, MartenContext>();
            builder.Services.AddScoped<Domain.Data.IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<Domain.Data.IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<Domain.Data.ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<Domain.Data.IAdvertisementRepository, AdvertisementRepository>();


            return builder;
        }
    }

    public class SeedData : IInitialData
    {
        private readonly object[] _initialData;

        public SeedData()
        {
            var category = Category.CreateNewCategory("Beauty", "MakeUp", "Maquiagem", "Acessórios para rosto");

            _initialData = new object[] { category };
        }

        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            await using var session = store.LightweightSession();
            session.Store(_initialData);
            session.QueueSqlCommand(CustomQueries.AlterAdvertisementTableAddGeoPoint);
            await session.SaveChangesAsync();
        }
    }

    #region MartenRegistry
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

    public class AdvertisementRegistry : MartenRegistry
    {
        public AdvertisementRegistry()
        {
            For<Advertisement>()
                .Index(x => x.Id)
                .ForeignKey<Category>(x => x.CategoryId)
                .ForeignKey<Account>(x => x.OwnerId);
        }
    }
    #endregion

    public class HasSmartEnumValueParser<TEnum> : IMethodCallParser where TEnum : SmartEnum<TEnum, int>
    {
        public bool Matches(MethodCallExpression expression)
        {
            // Match any method call to `HasSmartEnumValue`
            return expression.Method.Name == nameof(UniLx.Shared.LibExtensions.SmartEnumExtensions.HasSmartEnumValue);
        }

        public ISqlFragment Parse(IQueryableMemberCollection memberCollection, IReadOnlyStoreOptions options, MethodCallExpression expression)
        {
            // Use NullTestLocator if available to ensure proper `->>` access for JSON text retrieval
            var locator = memberCollection.MemberFor(expression.Arguments[0]).NullTestLocator;

            // If NullTestLocator is null or not properly set, fallback to JSONBLocator
            if (string.IsNullOrEmpty(locator))
            {
                locator = memberCollection.MemberFor(expression.Arguments[0]).JSONBLocator;
            }

            // Extract the target SmartEnum value from the second argument
            var targetEnumValue = (TEnum)Expression.Lambda(expression.Arguments[1]).Compile().DynamicInvoke();

            // Generate the SQL fragment using the adjusted locator and target SmartEnum name
            return new WhereFragment($"{locator} = '{targetEnumValue.Name}'");
        }
    }
}
