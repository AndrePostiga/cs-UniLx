using UniLx.Shared.Abstractions;

namespace UniLx.Application.Usecases.Advertisements
{
    internal static class AdvertisementErrors
    {
        public static readonly Error NotFound = new(System.Net.HttpStatusCode.NotFound, "Advertisement.Id.NotFound", "Can't found an advertisement with provided id.");
        public static readonly Error AccountNotFound = new(System.Net.HttpStatusCode.NotFound, "Advertisement.Owner.NotFound", "Can't found owner for advertisement.");
    }
}
