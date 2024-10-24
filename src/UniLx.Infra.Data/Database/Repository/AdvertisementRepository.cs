using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(IMartenContext martenContext, IUnitOfWork unitOfWork) : base(martenContext, unitOfWork)
        {
        }
    }
}
