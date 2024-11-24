using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class PetType : SmartEnum<PetType>
    {
        public static readonly PetType Sell = new PetType("Sell".ToSnakeCase(), 1);
        public static readonly PetType Adoption = new PetType("Adoption".ToSnakeCase(), 2);
        public static readonly PetType Accessories = new PetType("Accessories".ToSnakeCase(), 3);

        private PetType(string name, int value) : base(name, value) { }
    }
}
