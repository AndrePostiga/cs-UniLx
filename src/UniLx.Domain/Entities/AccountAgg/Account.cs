using System.Net;
using UniLx.Domain.Entities.AccountAgg.ValueObj;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AccountAgg
{
    public class Account : Entity
    {
        public string Name { get; private set; }

        public CPF Cpf { get; private set; }

        public string Description { get; private set; }

        public Email Email { get;  private set; }

        public StorageImage? ProfilePicture { get;  private set; }

        public Rating Rating { get; private set; }

        public IReadOnlyList<string>? AdvertisementIds => _advertisementIds!.AsReadOnly();
        private List<string>? _advertisementIds = [];

        private Account()
        {}

        public Account(string name, string email, string Cpf, string? description, string? profilePicture) : base(ProduceExternalId("account_"))
        {
            SetName(name);
            SetDescription(description);
            SetEmail(email);
            SetCpf(Cpf);
            Rating = new Rating();
            
            if (!string.IsNullOrWhiteSpace(profilePicture))
                ProfilePicture = StorageImage.CreatePrivateImage(profilePicture);
        }

        public void AddAdvertisement(Advertisement advertisement)
        {
            DomainException.ThrowIf(advertisement is null, "Advertisement cannot be null.");
            _advertisementIds!.Add(advertisement!.Id);
        }

        private void SetCpf(string cpf)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(cpf), "CPF cannot be null.");
            Cpf = new CPF(cpf!);
        }

        private void SetEmail(string email)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(email), "Email cannot be null.");
            Email = new Email(email!);
        }

        private void SetName(string name)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(name), "Name cannot be null.");
            DomainException.ThrowIf(name.Length > 100, "Name field must have 100 characters or less");
            Name = name;
        }

        private void SetDescription(string? description)
        {
            DomainException.ThrowIf(description?.Length > 256, "Description field must have 256 characters or less");

            if (description is not null)
                Description = WebUtility.HtmlEncode(description);
        }
    }
}
