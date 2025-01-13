using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg
{
    public abstract class Details
    {
        protected abstract AdvertisementType Type { get; }
        public string Title { get; protected set; }
        public string? Description { get; protected set; }
        public int? Price { get; protected set; }                
        public List<string>? Images { get; private set; }

        protected Details() { }

        protected Details(string? title, string? description, int? price)
        {
            SetTittle(title);
            SetDescription(description);
            SetPrice(price);
        }

        public new AdvertisementType GetType() => Type;

        protected virtual void SetPrice(int? price)
        {
            if (!price.HasValue)
                return;

            DomainException.ThrowIf(price.Value < 0, $"{nameof(Price)} cannot be less than 0");
            DomainException.ThrowIf(price.Value > 100_000_000, $"{nameof(Price)} cannot be more than R$ 1.000.000,00");
            Price = price.Value;
        }

        private void SetDescription(string? description)
        {
            DomainException.ThrowIf(description?.Length > 512, "Description field must have 512 characters or less");

            if (description is not null)
                Description = description;
        }

        private void SetTittle(string? title)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(title), $"{nameof(Title)} cannot be null.");
            DomainException.ThrowIf(title!.Length > 256, "nameof(Title)} field must have 256 characters or less");
            Title = title;
        }

        private void AddImageUrl(string? uri)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(uri), $"{nameof(uri)} cannot be null.");

            if (!Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                throw new DomainException($"{nameof(uri)} is not a valid URL.");
            }

            Images ??= [];   
            Images.Add(uri!.ToString());
        }

        public void AddImageUrls(IEnumerable<string?> uris)
        {
            DomainException.ThrowIf(uris == null, "The list of URIs cannot be null.");           

            foreach (var uri in uris!)
            {
                AddImageUrl(uri);
            }
        }
    }
}
