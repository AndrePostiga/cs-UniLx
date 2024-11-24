using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.Seedwork.ValueObj
{
    public class ContactInformation
    {
        public Phone? Phone { get; }
        public Email? Email { get; }
        public Uri? Website { get; }

        private ContactInformation() { }

        public ContactInformation(Phone? phone, Email? email, string? website)
        {
            DomainException.ThrowIf(phone == null && email == null, "Either Phone or Email must be provided.");

            Phone = phone;
            Email = email;
            Website = SetWebsite(website);
        }

        private static Uri? SetWebsite(string? website)
        {
            if (string.IsNullOrWhiteSpace(website)) return null;

            DomainException.ThrowIf(!Uri.TryCreate(website, UriKind.Absolute, out Uri? uriResult), "Invalid website URL.");
            return uriResult;
        }

        public override string ToString()
        {
            string phoneString = Phone != null ? $"Phone: {Phone}" : "Phone: Not provided";
            string emailString = Email != null ? $"Email: {Email}" : "Email: Not provided";
            string websiteString = Website != null ? $"Website: {Website}" : "Website: Not provided";

            return $"{phoneString}, {emailString}, {websiteString}";
        }
    }
}
