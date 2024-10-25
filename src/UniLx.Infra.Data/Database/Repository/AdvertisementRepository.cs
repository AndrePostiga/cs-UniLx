using UniLx.Domain.Data;
using UniLx.Domain.Entities;
using UniLx.Domain.Entities.AdvertisementAgg;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(IMartenContext martenContext, IUnitOfWork unitOfWork) : base(martenContext, unitOfWork)
        {                    
        }

        public async Task InsertOneWithLocation(Advertisement advertisement)
        {
            //var session = _martenContext.OpenSession();

            //session.QueryAsync()

            //InsertOne(advertisement);
            //Action<IDatabaseSession> insertLocationCommand = (session) =>
            //{
            //    session.
            //};
        }
    }
}
