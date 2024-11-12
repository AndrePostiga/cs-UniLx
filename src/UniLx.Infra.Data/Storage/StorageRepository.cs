using Microsoft.Extensions.Options;
using Supabase.Storage;
using Supabase.Storage.Exceptions;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Infra.Data.Storage.Buckets;

namespace UniLx.Infra.Data.Storage
{
    public interface IStorageRepository<TSettings>
    {
        Task<UploadSignedUrl> GeneratePreSignedUrlAsync(string fileName, int expiresInSeconds);
        Task<string?> GetImageUrl(StorageImage? storageImage, DateTime? expiresAt = null);
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

        public async Task<string?> GetImageUrl(StorageImage? storageImage, DateTime? expiresAt = null)
        {
            if (storageImage is null)
                return null;

            var imageFullPath = $"{_settings.Folder}/{storageImage.FullPath}";
            var expirationInSeconds = CalculateExpirationInSeconds(expiresAt);

            try
            {
                if (storageImage.IsPrivate)
                {
                    return await _supabaseClient
                        .Storage
                        .From(_settings.BucketName!)
                        .CreateSignedUrl(imageFullPath, expirationInSeconds);
                }

                return await Task.Run(() => _supabaseClient
                        .Storage
                        .From(_settings.BucketName!)
                        .GetPublicUrl(imageFullPath));
            }
            catch (SupabaseStorageException ex)
            {
                if (ex.Reason == FailureHint.Reason.NotFound)
                    return null;

                throw;
            }
        }

        private int CalculateExpirationInSeconds(DateTime? expiresAt)
        {
            if (!expiresAt.HasValue)
                return (int)TimeSpan.FromMinutes(5).TotalSeconds;

            // Calculate expiration in seconds from now
            var timeSpan = expiresAt.Value - DateTime.UtcNow;
            var expirationInSeconds = (int)timeSpan.TotalSeconds;

            // Ensure expiration time is positive, otherwise default to 5 minutes
            if (expirationInSeconds <= 0)
                expirationInSeconds = (int)TimeSpan.FromMinutes(5).TotalSeconds;

            return expirationInSeconds;
        }
    }
}
