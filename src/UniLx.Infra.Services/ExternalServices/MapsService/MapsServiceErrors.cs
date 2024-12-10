using UniLx.Shared.Abstractions;

namespace UniLx.Infra.Services.ExternalServices.MapsService
{
    internal static class MapsServiceErrors
    {
        public static readonly Error CannotResolve = new(System.Net.HttpStatusCode.BadRequest, "Address.CannotResolve", "Cannot resolve address check request.");
        public static Error Generic(string message) => new(System.Net.HttpStatusCode.BadGateway, "Address.CannotResolve", message);
    }
}
