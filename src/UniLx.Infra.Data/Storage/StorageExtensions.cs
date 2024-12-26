using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;
using UniLx.Infra.Data.Database.Options;
using UniLx.Infra.Data.Storage.Buckets;

namespace UniLx.Infra.Data.Storage
{
    [ExcludeFromCodeCoverage]
    public static class StorageExtensions
    {

        public static WebApplicationBuilder AddStorage(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<AccountBucketOptions>(builder.Configuration.GetSection(AccountBucketOptions.Section));
            builder.Services.Configure<AdvertisementBucketOptions>(builder.Configuration.GetSection(AdvertisementBucketOptions.Section));
            builder.Services.Configure<S3Options>(builder.Configuration.GetSection(S3Options.Section));


            builder.Services.AddScoped<IStorageRepository<AccountBucketOptions>, StorageRepository<AccountBucketOptions>>();
            builder.Services.AddScoped<IStorageRepository<AdvertisementBucketOptions>, StorageRepository<AdvertisementBucketOptions>>();
            
            builder.Services.AddSingleton<IAmazonS3>(sp =>
            {
                var options = sp.GetService<IOptions<S3Options>>()!.Value;
                return new AmazonS3Client( 
                awsAccessKeyId: options.AccessKeyId,
                awsSecretAccessKey: options.SecretAccessKey,
                clientConfig: new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.GetBySystemName(options.Region)
                });
            });

            return builder;
        }
    }
}
