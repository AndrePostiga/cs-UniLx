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
using System.Diagnostics.CodeAnalysis;
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
    [ExcludeFromCodeCoverage]
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
                opts.AutoCreateSchemaObjects = AutoCreate.All;

                opts.Schema.Include<AccountRegistry>();
                opts.Schema.Include<CategoryRegistry>();
                opts.Schema.Include<AdvertisementRegistry>();

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
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<ProductCondition, int>());
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<FashionGender, int>());
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<FashionSize, int>());
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<WorkLocationType, int>());
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<EmploymentType, int>());
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<PetGender, int>());
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<PetType, int>());

                        serializerOptions.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
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

    [ExcludeFromCodeCoverage]
    public class SeedData : IInitialData
    {
        private readonly object[] _initialData;        

        public SeedData()
        {
            var beautyCategories = new[]
            {
                Category.CreateNewCategory(AdvertisementType.Beauty.Name, "makeup", "Maquiagem", "Acessórios para rosto"),
                Category.CreateNewCategory(AdvertisementType.Beauty.Name, "skincare", "Cuidados com a Pele", "Produtos de cuidado facial e corporal"),
                Category.CreateNewCategory(AdvertisementType.Beauty.Name, "haircare", "Cuidados com o Cabelo", "Shampoos, condicionadores e produtos para cabelo"),
                Category.CreateNewCategory(AdvertisementType.Beauty.Name, "fragrances", "Perfumes e Fragrâncias", "Perfumes e sprays corporais"),
                Category.CreateNewCategory(AdvertisementType.Beauty.Name, "nailcare", "Cuidados com Unhas", "Esmaltes, removedores e acessórios"),
                Category.CreateNewCategory(AdvertisementType.Beauty.Name, "tools", "Ferramentas e Acessórios", "Pincéis, aplicadores e ferramentas de beleza"),
                Category.CreateNewCategory(AdvertisementType.Beauty.Name, "bath_body", "Banho e Corpo", "Sabonetes, loções e produtos para banho"),
                Category.CreateNewCategory(AdvertisementType.Beauty.Name, "men_grooming", "Cuidados Masculinos", "Produtos de barbear e cuidados masculinos")
            };

            var electronicsCategories = new[]
            {
                Category.CreateNewCategory(AdvertisementType.Electronics.Name, "smartphones", "Smartphones", "Celulares e acessórios de última geração"),
                Category.CreateNewCategory(AdvertisementType.Electronics.Name, "laptops", "Notebooks", "Laptops para trabalho, estudo e jogos"),
                Category.CreateNewCategory(AdvertisementType.Electronics.Name, "pcs", "Computadores", "Desktops e acessórios para escritório"),
                Category.CreateNewCategory(AdvertisementType.Electronics.Name, "videogames", "Videogames e Consoles", "Consoles e jogos para diversão"),
                Category.CreateNewCategory(AdvertisementType.Electronics.Name, "audio_devices", "Dispositivos de Áudio", "Fones de ouvido, caixas de som e mais"),
                Category.CreateNewCategory(AdvertisementType.Electronics.Name, "wearables", "Tecnologia Vestível", "Smartwatches, rastreadores de fitness e mais"),
                Category.CreateNewCategory(AdvertisementType.Electronics.Name, "cameras", "Câmeras e Fotografia", "Câmeras digitais, DSLRs e acessórios"),
                Category.CreateNewCategory(AdvertisementType.Electronics.Name, "drones", "Drones", "Drones para fotografia e recreação")
            };

            var eventsCategories = new[]
            {
                Category.CreateNewCategory(AdvertisementType.Events.Name, "concerts", "Shows e Concertos", "Eventos musicais ao vivo e festivais"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "workshops", "Workshops e Cursos", "Cursos e oficinas de aprendizado"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "sports_outdoors", "Esportes e Atividades ao Ar Livre", "Atividades esportivas e ao ar livre"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "exhibitions_fairs", "Exposições e Feiras", "Exposições de arte e feiras de diversos setores"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "conferences", "Conferências e Palestras", "Seminários, conferências e eventos de networking"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "parties_nightlife", "Festas e Entretenimento Noturno", "Baladas, festas e eventos noturnos"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "religious_spiritual", "Eventos Religiosos e Espirituais", "Retiros e celebrações religiosas"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "cinema_theater", "Cinema e Teatro", "Apresentações de filmes, peças e comédia"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "community_meetups", "Encontros e Comunidades", "Reuniões de clubes e encontros de grupos"),
                Category.CreateNewCategory(AdvertisementType.Events.Name, "festivals_fairs", "Festivais Culturais e Feiras de Rua", "Eventos culturais e feiras locais")
            };

            var fashionCategories = new[]
            {
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "mens_clothing", "Roupas Masculinas", "Camisas, calças, jaquetas e mais"),
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "womens_clothing", "Roupas Femininas", "Vestidos, saias, blusas e mais"),
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "unisex", "Roupas Unissex", "Estilos que servem para todos os gêneros"),
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "accessories", "Acessórios", "Relógios, chapéus, bolsas e mais"),
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "footwear", "Calçados", "Sapatos, tênis, botas e mais"),
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "sportswear", "Roupas Esportivas", "Roupas para atividades físicas e esportes"),
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "kids_clothing", "Roupas Infantis", "Roupas para crianças de todas as idades"),
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "luxury", "Moda de Luxo", "Estilos de alta costura e marcas premium"),
                Category.CreateNewCategory(AdvertisementType.Fashion.Name, "traditional", "Roupas Tradicionais", "Trajes típicos e roupas culturais")
            };

            var jobOpportunitiesCategories = new[]
            {
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "it_software", "TI e Software", "Vagas para desenvolvedores, engenheiros de software e profissionais de tecnologia"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "healthcare", "Saúde e Medicina", "Oportunidades para médicos, enfermeiros e profissionais de saúde"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "education", "Educação", "Vagas para professores, tutores e profissionais da educação"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "finance", "Finanças e Contabilidade", "Posições para contadores, analistas financeiros e similares"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "marketing", "Marketing e Publicidade", "Trabalhos relacionados a marketing, branding e publicidade"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "construction", "Construção Civil", "Oportunidades em construção, engenharia civil e arquitetura"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "hospitality", "Hotelaria e Turismo", "Vagas em hotéis, turismo e serviços relacionados"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "retail", "Varejo e Atendimento ao Cliente", "Posições em lojas, vendas e suporte ao cliente"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "transportation", "Transporte e Logística", "Trabalhos em logística, transporte e cadeia de suprimentos"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "freelance", "Freelance", "Oportunidades independentes e temporárias"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "government", "Setor Público", "Vagas em serviços públicos e governo"),
                Category.CreateNewCategory(AdvertisementType.JobOpportunities.Name, "remote", "Trabalho Remoto", "Oportunidades para trabalhar remotamente de qualquer lugar")
            };

            var petsCategories = new[]
            {
                Category.CreateNewCategory(AdvertisementType.Pets.Name, "accessories", "Acessórios para Animais", "Coleiras, camas, brinquedos, gaiolas e mais"),
                Category.CreateNewCategory(AdvertisementType.Pets.Name, "adoption", "Adoção", "Animais disponíveis para adoção"),
                Category.CreateNewCategory(AdvertisementType.Pets.Name, "selling", "Venda", "Animais disponíveis para venda"),
            };

            _initialData = [
                .. beautyCategories, 
                .. electronicsCategories, 
                .. eventsCategories,
                .. fashionCategories,
                .. jobOpportunitiesCategories,
                .. petsCategories
            ];
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
    [ExcludeFromCodeCoverage]
    public class AccountRegistry : MartenRegistry
    {
        public AccountRegistry()
        {
            For<Account>()
                .Identity(x => x.Id)
                .Duplicate(x => x.Cpf.Value, pgType: "varchar(20)", notNull: true)
                .Duplicate(x => x.Email.Value, pgType: "varchar(128)", notNull: true);
        }
    }

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
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

    [ExcludeFromCodeCoverage]
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
