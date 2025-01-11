using System.Linq.Expressions;
using UniLx.Domain.Data;
using UniLx.Domain.Entities.AccountAgg;
using UniLx.Infra.Data.Storage;
using UniLx.Infra.Data.Storage.Buckets;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class AccountRepository : Repository<Account>, IAccountRepository
    {
        private readonly IStorageRepository<AccountBucketOptions> _storageRepository;

        public AccountRepository(IMartenContext martenContext,
            IUnitOfWork unitOfWork,
            IStorageRepository<AccountBucketOptions> storageRepository)
            : base(martenContext, unitOfWork)
        {
            _storageRepository = storageRepository;
        }

        public async override Task<Tuple<IEnumerable<Account>?, int>> FindAll(int skip, int limit, bool sortAsc, Expression<Func<Account, bool>> expression, CancellationToken ct)
        {
            var martenFinds = await base.FindAll(skip, limit, sortAsc, expression, ct);

            if (martenFinds.Item1 == null)
                return martenFinds;

            var accounts = martenFinds.Item1!.Select(async accounts =>
            {
                var image = await _storageRepository.GetMostRecentFileAsync(accounts.Id);
                if (image is not null)
                    accounts.UpdateProfilePicture(image);
            });

            await Task.WhenAll(accounts);
            return martenFinds;
        }

        public async override Task<Account?> FindOne(Expression<Func<Account, bool>> expression, CancellationToken ct)
        {
            var martenFind = await base.FindOne(expression, ct);

            if (martenFind is null)
                return martenFind;

            var image = await _storageRepository.GetMostRecentFileAsync(martenFind.Id);

            if (image is not null)
                martenFind.UpdateProfilePicture(image);

            return martenFind;
        }
    }
}
