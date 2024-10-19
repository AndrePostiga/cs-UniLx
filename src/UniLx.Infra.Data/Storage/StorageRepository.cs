using Microsoft.Extensions.Options;
using Supabase.Storage;
using Supabase.Storage.Exceptions;
using UniLx.Infra.Data.Storage.Buckets;

namespace UniLx.Infra.Data.Storage
{
    public interface IStorageRepository<TSettings>
    {
        Task<UploadSignedUrl> GeneratePreSignedUrlAsync(string fileName, int expiresInSeconds);
    }

    public class StorageRepository<TSettings> : IStorageRepository<TSettings>
        where TSettings : class, IBucketSettings
    {
        private readonly TSettings _settings;
        protected readonly Supabase.Client _supabaseClient;

        public StorageRepository(IOptions<TSettings> settings, Supabase.Client supabaseClient)
        {
            _settings = settings.Value;
            _supabaseClient = supabaseClient;
        }

        public async Task<UploadSignedUrl> GeneratePreSignedUrlAsync(string fileName, int expiresInSeconds)
        {
            var fullPath = $"{_settings.Folder}/{fileName}";

            var signedUrl = await _supabaseClient
            .Storage
            .From(_settings.BucketName!)
            .CreateUploadSignedUrl(fullPath);

            return signedUrl;
        }
    }

}
