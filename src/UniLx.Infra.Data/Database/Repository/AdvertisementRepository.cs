using UniLx.Domain.Data;
using UniLx.Domain.Entities.AdvertisementAgg;

namespace UniLx.Infra.Data.Database.Repository
{
    internal class AdvertisementRepository : Repository<Advertisement>, IAdvertisementRepository
    {
        public AdvertisementRepository(IMartenContext martenContext, IUnitOfWork unitOfWork) : base(martenContext, unitOfWork)
        {                    
        }

        public override void InsertOne(Advertisement advertisement)
        {
            base.InsertOne(advertisement);

            if (advertisement.Address is not null && advertisement.Address.HasCordinates)
                base.CustomSql(CustomQueries.InsertGeopointOnAdvertisementTable, 
                    advertisement.Address.Latitude!, 
                    advertisement.Address.Longitude!,
                    advertisement.Id);
        }
    }
}
