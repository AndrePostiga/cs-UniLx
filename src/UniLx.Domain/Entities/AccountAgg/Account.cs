using System.Net;
using UniLx.Domain.Entities.AccountAgg.ValueObj;
using UniLx.Domain.Entities.AdvertisementAgg;
using UniLx.Domain.Entities.AdvertisementAgg.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AccountAgg
{
    public class Account : Entity
    {
        public string Name { get; private set; }

        public CPF Cpf { get; private set; }

        public string Description { get; private set; }

        public Email Email { get; private set; }

        public Image ProfilePicture { get; private set; }

        public Rating Rating { get; private set; }

        public IReadOnlyList<string>? AdvertisementIds => _advertisementIds!.AsReadOnly();
        private List<string>? _advertisementIds = [];

        public Account(string name, Email email, CPF Cpf, Image image) : base(ProduceExternalId("acc_"))
        {
            SetName(name);
            SetDescription(Description);
            SetEmail(email);
            SetCpf(Cpf);
            ProfilePicture = image;
            Rating = new Rating();
        }

        public void AddAdvertisement(Advertisement advertisement)
        {
            DomainException.ThrowIf(advertisement is null, "Advertisement cannot be null.");
            _advertisementIds!.Add(advertisement!.Id);
        }

        private void SetCpf(CPF cpf)
        {
            DomainException.ThrowIf(cpf is null, "CPF cannot be null.");
            Cpf = cpf!;
        }

        private void SetEmail(Email email)
        {
            DomainException.ThrowIf(email is null, "Email cannot be null.");
            Email = email!;
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
