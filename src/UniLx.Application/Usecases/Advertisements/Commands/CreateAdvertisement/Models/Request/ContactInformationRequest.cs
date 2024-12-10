using UniLx.Application.Usecases.SharedModels.Requests;

namespace UniLx.Application.Usecases.Advertisements.Commands.CreateAdvertisement.Models.Request
{
    public class ContactInformationRequest
    {
        public PhoneRequest? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
    }
}
