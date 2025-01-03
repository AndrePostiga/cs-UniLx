using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements
{
    internal static class AdvertisementErrors
    {
        public static readonly Error NotFound = new(System.Net.HttpStatusCode.NotFound, "Advertisement.Id.NotFound", "Can't found an advertisement with provided id.");
        public static readonly Error AccountNotFound = new(System.Net.HttpStatusCode.NotFound, "Advertisement.Owner.NotFound", "Can't found owner for advertisement.");
        public static readonly Error ErrorGeneratingPresignUrl = new(System.Net.HttpStatusCode.BadGateway, "Advertisement.PresignUrl", "Error when generating presign url for advertisement.");
        public static readonly Error InvalidStatusForRating = new(System.Net.HttpStatusCode.BadGateway, "Advertisement.InvalidStatusForRating", "Error when try to rate, invalid status for rating.");
    }
}
