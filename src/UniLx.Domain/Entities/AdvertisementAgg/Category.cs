using System.Net;
using UniLx.Domain.Entities.AdvertisementAgg.ValueObj;
using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AdvertisementAgg
{
    public class Category : Entity
    {
        public Category Root { get; private set; }

        public IReadOnlyList<Category>? Subcategories => _subcategories?.AsReadOnly();
        private List<Category>? _subcategories;

        public string Name { get; private set; }
        public string NameInPtBr { get; private set; }
        public string? Description { get; private set; }
        public Image IconUrl { get; private set; }   
        public bool IsActive { get; private set; }
        public bool IsDeleted { get; private set; }

        public static Category CreateNewCategory(Category root, string name, string nameInPtBr, string? description, Image image)
            => new(root, name, nameInPtBr, description, image);

        private Category(Category root, string name, string nameInPtBr, string? description, Image image) : base(ProduceExternalId("cat_"))
        {
            Root = root;
            SetName(name);
            SetNameInPtBr(nameInPtBr);
            SetDescription(description);
            IconUrl = image;
            IsActive = true;
        }

        private void SetName(string name)
        {            
            DomainException.ThrowIf(string.IsNullOrWhiteSpace(name), "Name cannot be null.");
            DomainException.ThrowIf(name.Length > 100, "Name field must have 100 characters or less");            
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
