using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;

namespace UniLx.Infra.Data.ServiceExtensions
{
    [ExcludeFromCodeCoverage]
    public static class StorageExtensions
    {

        public static WebApplicationBuilder AddStorage(this WebApplicationBuilder builder) 
        {

            builder.Services.Configure<AccountAvatarBucketOptions>(builder.Configuration.GetSection(AccountAvatarBucketOptions.Section));
            builder.Services.AddScoped<IStorageRepository<AccountAvatarBucketOptions>, StorageRepository<AccountAvatarBucketOptions>>();

            return builder;
        }
    }
}
