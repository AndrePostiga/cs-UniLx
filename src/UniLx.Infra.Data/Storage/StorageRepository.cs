using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using UniLx.Infra.Data.Storage.Buckets;

namespace UniLx.Infra.Data.Storage
{
    public interface IStorageRepository<TSettings>
    {
        Task<string?> GeneratePreSignedUrlAsync(string path, DateTime expiresAt);
        Task<string?> GetMostRecentFileAsync(string subFolder);
        Task<List<string>> ListFilesAsync(string? subFolder = null);
    }

    public class StorageRepository<TSettings> : IStorageRepository<TSettings>
        where TSettings : class, IBucketSettings
    {
        private readonly TSettings _settings;
        private readonly IAmazonS3 _s3Client;

        public StorageRepository(IOptions<TSettings> settings, IAmazonS3 s3Client)
        {
            _settings = settings.Value;
            _s3Client = s3Client;
        }

        public async Task<List<string>> ListFilesAsync(string? subFolder = null)
        {
            var prefix = BuildPrefix(subFolder);

            var listResponse = await GetS3ObjectsAsync(prefix);
            
            var fileKeys = listResponse.Where(s3Object => !s3Object.Key.EndsWith("/"));

            var tasks = fileKeys.Select(s3Object => GenerateUrlAsync(s3Object.Key, _settings.IsPrivate));
            var urls = await Task.WhenAll(tasks);

            return [.. urls];
        }

        public async Task<string?> GeneratePreSignedUrlAsync(string path, DateTime expiresAt)
        {
            var fullPath = $"{_settings.Folder}/{path}";
            return await GeneratePreSignedUrl(fullPath, HttpVerb.PUT, expiresAt);
        }

        public async Task<string?> GetMostRecentFileAsync(string subFolder)
        {
            var prefix = BuildPrefix(subFolder);

            var listResponse = await GetS3ObjectsAsync(prefix);
            var mostRecentObject = listResponse
                .OrderByDescending(obj => obj.LastModified)
                .FirstOrDefault();

            if (mostRecentObject == null)
                return null;

            return await GenerateUrlAsync(mostRecentObject.Key, _settings.IsPrivate);
        }

        private string BuildPrefix(string? subFolder)
        {
            if (!string.IsNullOrWhiteSpace(subFolder))
                return $"{_settings.Folder}/{subFolder}";

            return _settings.Folder!;
        }

        private async Task<IEnumerable<S3Object>> GetS3ObjectsAsync(string prefix)
        {
            var listRequest = new ListObjectsV2Request
            {
                BucketName = _settings.BucketName,
                Prefix = $"{prefix}/"
            };

            var listResponse = await _s3Client.ListObjectsV2Async(listRequest);
            return listResponse.S3Objects;
        }

        private async Task<string?> GeneratePreSignedUrl(string fullPath, HttpVerb verb, DateTime expiration)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _settings.BucketName,
                Key = fullPath,
                Verb = verb,
                Expires = expiration
            };

            return await _s3Client.GetPreSignedURLAsync(request);
        }

        private async Task<string?> GenerateUrlAsync(string key, bool isPrivate)
        {
            if (isPrivate)
                return await GeneratePreSignedUrl(key, HttpVerb.GET, DateTime.UtcNow.AddMinutes(30));            
            
            return await Task.FromResult($"https://{_settings.BucketName}.s3.amazonaws.com/{key}");            
        }
    }
}
