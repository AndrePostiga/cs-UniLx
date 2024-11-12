using System.Linq.Expressions;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Shared.LibExtensions;

namespace UniLx.Application.Usecases.Advertisements.Queries.GetAdvertisement.Spec
{
    internal static class AdvertisementsSpec
    {
        public static Expression<Func<Advertisement, bool>> ToSpec(this GetAdvertisementsQuery query) 
        {            
            return string.IsNullOrWhiteSpace(query.CategoryName) ?
                ToSpecWithType(query) :
                ToSpecWithCategory(query);
        }

        private static Expression<Func<Advertisement, bool>> ToSpecWithCategory(GetAdvertisementsQuery query) 
            =>  ad => ad.Status.HasSmartEnumValue(AdvertisementStatus.Created) && ad.CategoryName == query.CategoryName!.Trim();

        private static Expression<Func<Advertisement, bool>> ToSpecWithType(GetAdvertisementsQuery query) 
            =>  ad => ad.Status.HasSmartEnumValue(AdvertisementStatus.Created) && ad.Type.Name == query.Type!.Trim();
    }
}
