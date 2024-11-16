using Microsoft.Extensions.Options;
using System.Reflection.Emit;
using System.Text.Json;
using UniLx.Domain.Entities.Seedwork;
using UniLx.Domain.Services;
using UniLx.Infra.Services.ExternalServices.MapsService.Models;
using UniLx.Infra.Services.ExternalServices.MapsService.Options;
using UniLx.Shared.Abstractions;

namespace UniLx.Infra.Services.ExternalServices.MapsService
{
    internal class MapsService : IMapsService
    {
        private readonly HttpClient _client;
        private readonly IOptions<MapsApiOptions> _options;

        private static JsonSerializerOptions SerializerOptions => new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
        };

        public MapsService(HttpClient client, IOptions<MapsApiOptions> options)
        {
            _client = client;
            _options = options;
        }

        public async Task<ServiceResult<Address>> GetAddressFromCordinates(Address rawAddress, CancellationToken cancellationToken)
        {
            var requestUri = $"/maps/api/geocode/json?latlng={rawAddress.Latitude},{rawAddress.Longitude}&language=pt-BR&result_type=route&key={_options.Value.ApiKey}";
            return await FetchAndParseAddressAsync(requestUri, cancellationToken);
        }

        public async Task<ServiceResult<Address>> GetAddressFromFullAddress(Address rawAddress, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(rawAddress.FullAddress))
                return ServiceResult<Address>.Failure(MapsServiceErrors.Generic("The provided address is incomplete or invalid."));
            
            var addressParam = string.Join(", ", new[] { rawAddress.Number, rawAddress.Street, rawAddress.Neighborhood }.Where(part => !string.IsNullOrWhiteSpace(part)));

            var componentsParam = 
                        $"&components=postal_code:{Uri.EscapeDataString(rawAddress.ZipCode!)}" +
                        $"|country:{Uri.EscapeDataString(rawAddress.Country!)}" +
                        $"|administrative_area_level_1:{Uri.EscapeDataString(rawAddress.State!)}";

            var requestUri = $"/maps/api/geocode/json?address={Uri.EscapeDataString(addressParam)}{componentsParam}&key={_options.Value.ApiKey}";


            return await FetchAndParseAddressAsync(requestUri, cancellationToken);
        }

        private async Task<ServiceResult<Address>> FetchAndParseAddressAsync(string requestUri, CancellationToken cancellationToken)
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            using var response = await _client.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
                return ServiceResult<Address>.Failure(MapsServiceErrors.CannotResolve);

            var jsonResponse = await response.Content.ReadAsStringAsync(cancellationToken);
            var result = JsonSerializer.Deserialize<GetAddressByCordinatesResponse>(jsonResponse, SerializerOptions);

            if (result?.Status != "OK" || result.Results.Count == 0)
                return ServiceResult<Address>.Failure(MapsServiceErrors.CannotResolve);

            var firstResult = result.Results[0];
            var address = ParseAddressFromResult(firstResult);

            return ServiceResult.Success(address);
        }

        private static Address ParseAddressFromResult(ResultResponse result)
        {
            var responseLocation = result.Geometry.Location;
            var responseAddress = result.AddressComponents;

            return Address.CreateAddress(
                latitude: responseLocation.Lat,
                longitude: responseLocation.Lng,
                country: GetComponent(responseAddress, "country")?.ShortName,
                state: GetComponent(responseAddress, "administrative_area_level_1")?.ShortName,
                city: GetComponent(responseAddress, "administrative_area_level_2")?.LongName,
                zipCode: GetComponent(responseAddress, "postal_code")?.ShortName,
                neighborhood: 
                    GetComponent(responseAddress, "sublocality_level_1")?.ShortName ?? 
                    GetComponent(responseAddress, "administrative_area_level_3")?.ShortName ??
                    GetComponent(responseAddress, "administrative_area_level_4")?.ShortName,
                street: GetComponent(responseAddress, "route")?.LongName,
                number: GetComponent(responseAddress, "street_number")?.LongName);
        }

        private static AddressComponent? GetComponent(IEnumerable<AddressComponent> components, string type)
        {
            return components.FirstOrDefault(x => x.Types.Contains(type));
        }
    }
}
