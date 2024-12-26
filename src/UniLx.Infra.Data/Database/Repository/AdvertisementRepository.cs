using System.Linq.Expressions;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository
    {
        private readonly IStorageRepository<AdvertisementBucketOptions> _storageRepository;

        public AdvertisementRepository(IMartenContext martenContext, 
            IUnitOfWork unitOfWork, 
            IStorageRepository<AdvertisementBucketOptions> storageRepository) 
            : base(martenContext, unitOfWork)
        {
            _storageRepository = storageRepository;
        }

        public async override Task<Tuple<IEnumerable<Advertisement>?, int>> FindAll(int skip, int limit, bool sortAsc, Expression<Func<Advertisement, bool>> expression, CancellationToken ct)
        {
            var martenFinds = await base.FindAll(skip, limit, sortAsc, expression, ct);

            if (martenFinds.Item1 == null)
                return martenFinds;

            var advertisementTasks = martenFinds.Item1!.Select(async advertisement =>
            {
                var images = await _storageRepository.ListFilesAsync(advertisement.Id);
                if (images is not null)
                    advertisement.Details.AddImageUrls(images);
            });

            await Task.WhenAll(advertisementTasks);
            return martenFinds;
        }

        public async override Task<Advertisement?> FindOne(Expression<Func<Advertisement, bool>> expression, CancellationToken ct)
        {
            var martenFind = await base.FindOne(expression, ct);

            if (martenFind is null)
                return martenFind;

            var images = await _storageRepository.ListFilesAsync(martenFind.Id);

            if (images is not null)
                martenFind.Details.AddImageUrls(images);

            return martenFind;
        }
    }
}
