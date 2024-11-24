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

    public class SeedData : IInitialData
    {
        private readonly object[] _initialData;

        public SeedData()
        {
            var beautyCategories = new[]
            {
                Category.CreateNewCategory("beauty", "makeup", "Maquiagem", "Acessórios para rosto"),
                Category.CreateNewCategory("beauty", "skincare", "Cuidados com a Pele", "Produtos de cuidado facial e corporal"),
                Category.CreateNewCategory("beauty", "haircare", "Cuidados com o Cabelo", "Shampoos, condicionadores e produtos para cabelo"),
                Category.CreateNewCategory("beauty", "fragrances", "Perfumes e Fragrâncias", "Perfumes e sprays corporais"),
                Category.CreateNewCategory("beauty", "nailcare", "Cuidados com Unhas", "Esmaltes, removedores e acessórios"),
                Category.CreateNewCategory("beauty", "tools", "Ferramentas e Acessórios", "Pincéis, aplicadores e ferramentas de beleza"),
                Category.CreateNewCategory("beauty", "bath_body", "Banho e Corpo", "Sabonetes, loções e produtos para banho"),
                Category.CreateNewCategory("beauty", "men_grooming", "Cuidados Masculinos", "Produtos de barbear e cuidados masculinos")
            };

            var electronicsCategories = new[]
            {
                Category.CreateNewCategory("electronics", "smartphones", "Smartphones", "Celulares e acessórios de última geração"),
                Category.CreateNewCategory("electronics", "laptops", "Notebooks", "Laptops para trabalho, estudo e jogos"),
                Category.CreateNewCategory("electronics", "pcs", "Computadores", "Desktops e acessórios para escritório"),
                Category.CreateNewCategory("electronics", "videogames", "Videogames e Consoles", "Consoles e jogos para diversão"),
                Category.CreateNewCategory("electronics", "audio_devices", "Dispositivos de Áudio", "Fones de ouvido, caixas de som e mais"),
                Category.CreateNewCategory("electronics", "wearables", "Tecnologia Vestível", "Smartwatches, rastreadores de fitness e mais"),
                Category.CreateNewCategory("electronics", "cameras", "Câmeras e Fotografia", "Câmeras digitais, DSLRs e acessórios"),
                Category.CreateNewCategory("electronics", "drones", "Drones", "Drones para fotografia e recreação")
            };

            var eventsCategories = new[]
            {
                Category.CreateNewCategory("events", "concerts", "Shows e Concertos", "Eventos musicais ao vivo e festivais"),
                Category.CreateNewCategory("events", "workshops", "Workshops e Cursos", "Cursos e oficinas de aprendizado"),
                Category.CreateNewCategory("events", "sports_outdoors", "Esportes e Atividades ao Ar Livre", "Atividades esportivas e ao ar livre"),
                Category.CreateNewCategory("events", "exhibitions_fairs", "Exposições e Feiras", "Exposições de arte e feiras de diversos setores"),
                Category.CreateNewCategory("events", "conferences", "Conferências e Palestras", "Seminários, conferências e eventos de networking"),
                Category.CreateNewCategory("events", "parties_nightlife", "Festas e Entretenimento Noturno", "Baladas, festas e eventos noturnos"),
                Category.CreateNewCategory("events", "religious_spiritual", "Eventos Religiosos e Espirituais", "Retiros e celebrações religiosas"),
                Category.CreateNewCategory("events", "cinema_theater", "Cinema e Teatro", "Apresentações de filmes, peças e comédia"),
                Category.CreateNewCategory("events", "community_meetups", "Encontros e Comunidades", "Reuniões de clubes e encontros de grupos"),
                Category.CreateNewCategory("events", "festivals_fairs", "Festivais Culturais e Feiras de Rua", "Eventos culturais e feiras locais")
            };

            var fashionCategories = new[]
            {
                Category.CreateNewCategory("fashion", "mens_clothing", "Roupas Masculinas", "Camisas, calças, jaquetas e mais"),
                Category.CreateNewCategory("fashion", "womens_clothing", "Roupas Femininas", "Vestidos, saias, blusas e mais"),
                Category.CreateNewCategory("fashion", "unisex", "Roupas Unissex", "Estilos que servem para todos os gêneros"),
                Category.CreateNewCategory("fashion", "accessories", "Acessórios", "Relógios, chapéus, bolsas e mais"),
                Category.CreateNewCategory("fashion", "footwear", "Calçados", "Sapatos, tênis, botas e mais"),
                Category.CreateNewCategory("fashion", "sportswear", "Roupas Esportivas", "Roupas para atividades físicas e esportes"),
                Category.CreateNewCategory("fashion", "kids_clothing", "Roupas Infantis", "Roupas para crianças de todas as idades"),
                Category.CreateNewCategory("fashion", "luxury", "Moda de Luxo", "Estilos de alta costura e marcas premium"),
                Category.CreateNewCategory("fashion", "traditional", "Roupas Tradicionais", "Trajes típicos e roupas culturais")
            };

            var jobOpportunitiesCategories = new[]
            {
                Category.CreateNewCategory("job_opportunities", "it_software", "TI e Software", "Vagas para desenvolvedores, engenheiros de software e profissionais de tecnologia"),
                Category.CreateNewCategory("job_opportunities", "healthcare", "Saúde e Medicina", "Oportunidades para médicos, enfermeiros e profissionais de saúde"),
                Category.CreateNewCategory("job_opportunities", "education", "Educação", "Vagas para professores, tutores e profissionais da educação"),
                Category.CreateNewCategory("job_opportunities", "finance", "Finanças e Contabilidade", "Posições para contadores, analistas financeiros e similares"),
                Category.CreateNewCategory("job_opportunities", "marketing", "Marketing e Publicidade", "Trabalhos relacionados a marketing, branding e publicidade"),
                Category.CreateNewCategory("job_opportunities", "construction", "Construção Civil", "Oportunidades em construção, engenharia civil e arquitetura"),
                Category.CreateNewCategory("job_opportunities", "hospitality", "Hotelaria e Turismo", "Vagas em hotéis, turismo e serviços relacionados"),
                Category.CreateNewCategory("job_opportunities", "retail", "Varejo e Atendimento ao Cliente", "Posições em lojas, vendas e suporte ao cliente"),
                Category.CreateNewCategory("job_opportunities", "transportation", "Transporte e Logística", "Trabalhos em logística, transporte e cadeia de suprimentos"),
                Category.CreateNewCategory("job_opportunities", "freelance", "Freelance", "Oportunidades independentes e temporárias"),
                Category.CreateNewCategory("job_opportunities", "government", "Setor Público", "Vagas em serviços públicos e governo"),
                Category.CreateNewCategory("job_opportunities", "remote", "Trabalho Remoto", "Oportunidades para trabalhar remotamente de qualquer lugar")
            };

            var petsCategories = new[]
            {
                Category.CreateNewCategory("pets", "accessories", "Acessórios para Animais", "Coleiras, camas, brinquedos, gaiolas e mais"),
                Category.CreateNewCategory("pets", "adoption", "Adoção", "Animais disponíveis para adoção"),
                Category.CreateNewCategory("pets", "selling", "Venda", "Animais disponíveis para venda"),
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
