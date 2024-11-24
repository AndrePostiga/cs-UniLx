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


            builder.AddNpgsqlDataSource("postgresdb", ops =>
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
                opts.Linq.MethodCallParsers.Add(new HasSmartEnumValueParser<AdvertisementStatus>());
                opts.Linq.MethodCallParsers.Add(new HasSmartEnumValueParser<AdvertisementType>());

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
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<AgeRestriction, int>());
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
            var beautyCategory1 = Category.CreateNewCategory("beauty", "makeup", "Maquiagem", "Acessórios para rosto");
            var beautyCategory2 = Category.CreateNewCategory("beauty", "skincare", "Cuidados com a Pele", "Produtos de cuidado facial e corporal");

            var eventsCategory1 = Category.CreateNewCategory("events", "concerts", "Shows e Concertos", "Eventos musicais ao vivo e festivais");
            var eventsCategory2 = Category.CreateNewCategory("events", "workshops", "Workshops e Cursos", "Cursos e oficinas de aprendizado");
            var eventsCategory3 = Category.CreateNewCategory("events", "sports_outdoors", "Esportes e Atividades ao Ar Livre", "Atividades esportivas e ao ar livre");
            var eventsCategory4 = Category.CreateNewCategory("events", "exhibitions_fairs", "Exposições e Feiras", "Exposições de arte e feiras de diversos setores");
            var eventsCategory5 = Category.CreateNewCategory("events", "conferences", "Conferências e Palestras", "Seminários, conferências e eventos de networking");
            var eventsCategory6 = Category.CreateNewCategory("events", "parties_nightlife", "Festas e Entretenimento Noturno", "Baladas, festas e eventos noturnos");
            var eventsCategory7 = Category.CreateNewCategory("events", "religious_spiritual", "Eventos Religiosos e Espirituais", "Retiros e celebrações religiosas");
            var eventsCategory8 = Category.CreateNewCategory("events", "cinema_theater", "Cinema e Teatro", "Apresentações de filmes, peças e comédia");
            var eventsCategory9 = Category.CreateNewCategory("events", "community_meetups", "Encontros e Comunidades", "Reuniões de clubes e encontros de grupos");
            var eventsCategory10 = Category.CreateNewCategory("events", "festivals_fairs", "Festivais Culturais e Feiras de Rua", "Eventos culturais e feiras locais");

            _initialData = new object[]
            {
                beautyCategory1,
                beautyCategory2,
                eventsCategory1,
                eventsCategory2,
                eventsCategory3,
                eventsCategory4,
                eventsCategory5,
                eventsCategory6,
                eventsCategory7,
                eventsCategory8,
                eventsCategory9,
                eventsCategory10
            };
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
                .Index(x => new { x.Root, x.Id }, options =>
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
            // Ensure the method name matches
            if (expression.Method.Name != nameof(UniLx.Shared.LibExtensions.SmartEnumExtensions.HasSmartEnumValue))
            {
                return false;
            }

            // Ensure the argument matches the expected type
            var objectType = expression.Arguments[0].Type;
            return typeof(TEnum).IsAssignableFrom(objectType);
        }

        public ISqlFragment Parse(IQueryableMemberCollection memberCollection, IReadOnlyStoreOptions options, MethodCallExpression expression)
        {
            try
            {
                // Log the method for debugging
                Console.WriteLine($"Parsing expression: {expression}");

                // Get the locator for the member (Status or Type)
                var member = memberCollection.MemberFor(expression.Arguments[0]);
                var locator = member.NullTestLocator ?? member.JSONBLocator;

                if (string.IsNullOrEmpty(locator))
                {
                    throw new InvalidOperationException("Unable to resolve a valid locator for the member.");
                }

                // Resolve the generic argument dynamically
                var genericTypeArgument = expression.Method.GetGenericArguments()[0];

                // Extract the SmartEnum value dynamically
                var targetEnumValue = Expression.Lambda(expression.Arguments[1]).Compile().DynamicInvoke();

                if (targetEnumValue == null)
                {
                    throw new ArgumentException("Invalid SmartEnum value passed to HasSmartEnumValue.");
                }

                // Ensure the targetEnumValue is of the expected type
                if (!genericTypeArgument.IsInstanceOfType(targetEnumValue))
                {
                    throw new InvalidCastException($"Invalid SmartEnum value type. Expected: {genericTypeArgument}, Actual: {targetEnumValue.GetType()}");
                }

                // Generate the SQL fragment with parameterized input
                return new WhereFragment($"{locator} = ?", targetEnumValue.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HasSmartEnumValueParser.Parse: {ex.Message}");
                Console.WriteLine($"Expression: {expression}");
                throw;
            }
        }
    }
}
