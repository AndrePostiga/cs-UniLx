using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(IMartenContext martenContext, IUnitOfWork unitOfWork) : base(martenContext, unitOfWork)
        {
        }
    }
}
