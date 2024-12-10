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
        //public IReadOnlyList<Image>? Images => _images?.AsReadOnly();
        //private List<Image>? _images;

        protected Details() { }

        protected Details(string? title, string? description, int? price)
        {
            SetTittle(title);
            SetDescription(description);
            SetPrice(price);
        }

        public new AdvertisementType GetType() => Type;

        private void SetPrice(int? price)
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
    }
}
