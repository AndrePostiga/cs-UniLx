namespace UniLx.Application.Usecases.SharedModels.Responses.DetailsResponse
{
    public class PetDetailsResponse : BaseDetailsResponse
    {
        public string? PetType { get; set; }
        public string? AnimalType { get; set; }
        public int? Age { get; set; }
        public string? Breed { get; set; }
        public bool? IsVaccinated { get; set; }
        public string? Gender { get; set; }
        public bool? IsExotic { get; set; }

        public string? AccessoryType { get; set; }
        public List<string>? Materials { get; set; }

        public string? AdoptionRequirements { get; set; }
        public string? HealthStatus { get; set; }
        public bool? IsSterilized { get; set; }
    }
}
