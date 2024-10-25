using System.Net;
using System.Xml.Linq;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg
{
    public class Category : Entity
    {
        public AdvertisementType Root { get; private set; }
        public string Name { get; private set; }
        public string NameInPtBr { get; private set; }
        public string? Description { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        private Category() { }

        public static Category CreateNewCategory(string root, string name, string nameInPtBr, string? description)
            => new(root, name, nameInPtBr, description);

        private Category(string root, string name, string nameInPtBr, string? description) : base(ProduceExternalId("category_"))
        {
            SetRoot(root);
            SetName(name);
            SetNameInPtBr(nameInPtBr);
            SetDescription(description);
            IsActive = true;
        }

        private void SetRoot(string root)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(root), "root cannot be null.");

            var hasType = AdvertisementType.TryFromName(root, true, out var rootEnum);
            DomainException.ThrowIf(hasType is false, $"Invalid root type. Supported types are: {string.Join(", ", AdvertisementType.List)}");

            Root = rootEnum;
        }

        private void SetName(string name)
        {            
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(name), "Name cannot be null.");
            DomainException.ThrowIf(name.Length > 100, "Name field must have 100 characters or less");

            var hasType = AdvertisementType.TryFromName(name, true, out _);
            DomainException.ThrowIf(hasType, $"Invalid name. A category name cannot be the same as an advertisement type");

            Name = name;
        }

        private void SetNameInPtBr(string nameInPtBr)
        {
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(nameInPtBr), "Name cannot be null.");
            DomainException.ThrowIf(nameInPtBr.Length > 100, "Name in Pt-BR field must have 100 characters or less");
            NameInPtBr = nameInPtBr;
        }

        private void SetDescription(string? description)
        {
            DomainException.ThrowIf(description?.Length > 256, "Description field must have 256 characters or less");
            
            if (description is not null)
                Description = WebUtility.HtmlEncode(description);
        }
    }
}
