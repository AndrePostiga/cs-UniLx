using System.Diagnostics;
using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;
using UniLx.Domain.Entities.Seedwork.ValueObj;

namespace UniLx.Domain.Entities.AdvertisementAgg.SpecificDetails
{
    public class PetDetails : Details
    {
        protected override AdvertisementType Type => AdvertisementType.Pets;
        public string AnimalType { get; private set; }
        public int Age { get; private set; }
        public string Breed { get; private set; }
        public PetDetails(
            string title, string description, int price, IEnumerable<Image> images,
            string animalType, int age, string breed) : base(title, description, price)
        {
            AnimalType = animalType;
            Age = age;
            Breed = breed;
        }
    }
}
