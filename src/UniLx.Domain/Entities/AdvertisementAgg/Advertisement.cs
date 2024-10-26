using UniLx.Domain.Entities.AccountAgg;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg
{
    public class Advertisement : Entity
    {
        public AdvertisementStatus Status { get; private set; }
        public AdvertisementType Type { get; private set; }
        public Details Details { get; private set; }
        public DateTime ExpiresAt { get; private set; }
        public Address Address { get; private set; }
        public string OwnerId { get; private set; }
        public string CategoryId { get; private set; }
        public string CategoryName { get; private set; }

        private Advertisement() { }

        public Advertisement(string type, Category subCategory, Details details, DateTime? expiresAt, Address address, Account account) : base(ProduceExternalId("advertisement_"))
        {
            CategoryId = subCategory.Id;
            CategoryName = subCategory.Name;
            SetInitialStatus();
            SetExpiration(expiresAt);
            SetType(type, subCategory);
            SetDetails(details);
            SetOwner(account);
            SetAddress(address);
        }

        private void SetAddress(Address address)
        {
            DomainException.ThrowIf(address is null, "Cannot create advertisement without address.");
            Address = address!;
        }

        private void SetOwner(Account account)
        {
            DomainException.ThrowIf(account is null, "Cannot create advertisement without account.");
            OwnerId = account!.Id;
        }

        private void SetDetails(Details details)
        {
            DomainException.ThrowIf(details.GetType() != Type, "The details type must be the same as the advertisement type.");
            Details = details;
        }

        private void SetType(string type, Category subCategory)
        {
            var hasAddType = AdvertisementType.TryFromName(type, ignoreCase: true, out var adType);
            
            DomainException.ThrowIf(hasAddType == false, $"Invalid advertisement type, possible values are {string.Join(",", AdvertisementType.List)}.");

            DomainException.ThrowIf(adType.Name != subCategory.Root.Name, "The advertisement type must be the same as the category root.");

            Type = adType;
        }

        private void SetExpiration(DateTime? expiresAt)
        {
            var expAt = expiresAt ?? DateTime.UtcNow.AddDays(30);

            DomainException.ThrowIf(expAt < DateTime.UtcNow, "The expiration date must be greater than the current date.");

            DomainException.ThrowIf(expAt > DateTime.UtcNow.AddDays(90), "The expiration date must be up to 90 days.");

            ExpiresAt = expAt;
        }

        private void SetInitialStatus()
        {
            Status = AdvertisementStatus.Created;
        }
    }
}
