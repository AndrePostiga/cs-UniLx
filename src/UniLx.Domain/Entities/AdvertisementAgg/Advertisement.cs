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
        public string OwnerName { get; private set; }
        public string CategoryId { get; private set; }
        public string CategoryName { get; private set; }
        public Rating Rating { get; private set; }

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
            Rating = new Rating();
        }

        private void SetAddress(Address address)
        {
            DomainException.ThrowIf(address is null, "Cannot create advertisement without address.");
            DomainException.ThrowIf(address!.Country != "BR", "This service is only avaiable for Brazil.");
            DomainException.ThrowIf(address!.State != "RJ", "This service is only avaiable for state of Rio de Janeiro.");
            Address = address!;
        }

        private void SetOwner(Account account)
        {
            DomainException.ThrowIf(account is null, "Cannot create advertisement without account.");
            account!.AddAdvertisement(this);
            OwnerId = account!.Id;
            OwnerName = account!.Name;
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
            Status = AdvertisementStatus.Active;
        }

        public bool IsExpired()
        {
            return ExpiresAt < DateTime.UtcNow;
        }

        public void Rate(float rating, Account account, Account advertisementOwnerAccount)
        {
            DomainException.ThrowIf(account == null, "Account cannot be null.");
            DomainException.ThrowIf(advertisementOwnerAccount == null, "Account owner cannot be null.");

            if (Status.Name != AdvertisementStatus.Finished.Name && Status.Name != AdvertisementStatus.Expired.Name)
            {
                throw new DomainException("Only finished or expired advertisements can be rated.");
            }


            if (!account!.InterestAdvertisementIds.Contains(Id))
            {
                throw new DomainException("This account is not authorized to rate this advertisement.");
            }

            Rating.UpdateRating(rating);
            advertisementOwnerAccount!.Rating.UpdateRating(rating);
        }

        public void Finish(Account advertisementOwner)
        {
            DomainException.ThrowIf(advertisementOwner == null, "Advertisement owner cannot be null.");
            DomainException.ThrowIf(OwnerId != advertisementOwner!.Id, "The advertisement can only be finished by its owner.");
            DomainException.ThrowIf(!Status.CanChangeTo(AdvertisementStatus.Finished), "Only active advertisements can be finished.");         

            Status = AdvertisementStatus.Finished;
        }

        public bool IsActive() => Status.Name == AdvertisementStatus.Active.Name;
    }
}
