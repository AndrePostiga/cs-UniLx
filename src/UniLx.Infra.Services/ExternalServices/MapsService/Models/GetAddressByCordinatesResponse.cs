namespace UniLx.Infra.Services.ExternalServices.MapsService.Models
{
    internal class GetAddressByCordinatesResponse
    {
        public PlusCode PlusCode { get; set; } // Added to match the "plus_code" in JSON
        public List<ResultResponse> Results { get; set; }
        public string Status { get; set; }
    }

    internal class PlusCode
    {
        public string CompoundCode { get; set; }
        public string GlobalCode { get; set; }
    }

    internal class ResultResponse
    {
        public List<AddressComponent> AddressComponents { get; set; }
        public string FormattedAddress { get; set; }
        public Geometry Geometry { get; set; }
        public string PlaceId { get; set; }
        public PlusCode PlusCode { get; set; }
        public List<string> Types { get; set; }
    }

    internal class AddressComponent
    {
        public string LongName { get; set; }
        public string ShortName { get; set; }
        public List<string> Types { get; set; }
    }

    internal class Northeast
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    internal class Southwest
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    internal class Bounds
    {
        public Northeast Northeast { get; set; }
        public Southwest Southwest { get; set; }
    }

    internal class Location
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    internal class Viewport
    {
        public Northeast Northeast { get; set; }
        public Southwest Southwest { get; set; }
    }

    internal class Geometry
    {
        public Bounds Bounds { get; set; }
        public Location Location { get; set; }
        public string LocationType { get; set; }
        public Viewport Viewport { get; set; }
    }
}
