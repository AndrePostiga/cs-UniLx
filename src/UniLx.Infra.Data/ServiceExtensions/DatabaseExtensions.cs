﻿using Ardalis.SmartEnum.JsonNet;
using Marten;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Infra.Data.Database;
using UniLx.Infra.Data.Database.Repository;
using Weasel.Core;

namespace UniLx.Infra.Data.ServiceExtensions
{
    public static class DatabaseExtensions
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

                opts.Schema.Include<AccountRegistry>();

                opts.Policies.ForAllDocuments(x =>
                {
                    x.Metadata.CausationId.Enabled = true;
                    x.Metadata.CorrelationId.Enabled = true;
                    x.Metadata.Headers.Enabled = true;
                });

                opts.UseNewtonsoftForSerialization(
                    casing: Casing.SnakeCase,
                    enumStorage: EnumStorage.AsString,
                    nonPublicMembersStorage: NonPublicMembersStorage.NonPublicDefaultConstructor | NonPublicMembersStorage.NonPublicSetters,
                    configure: (serializerOptions) =>
                    {
                        serializerOptions.Converters.Add(new SmartEnumNameConverter<StorageType, int>());
                    }); 

            })
            .ApplyAllDatabaseChangesOnStartup()
            .UseLightweightSessions()
            .UseNpgsqlDataSource();

            builder.Services.AddSingleton<IMartenContext, MartenContext>();
            builder.Services.AddScoped<Domain.Data.IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<Domain.Data.IAccountRepository, AccountRepository>();

            return builder;
        }
    }

    public class AccountRegistry : MartenRegistry
    {
        public AccountRegistry()
        {
            For<Account>()
                .Identity(x => x.Id);
        }
    }
}
