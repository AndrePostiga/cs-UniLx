using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class PetGender : SmartEnum<PetGender>
    {
        public static readonly PetGender Male = new PetGender("Male".ToSnakeCase().ToLower(), 1);
        public static readonly PetGender Female = new PetGender("Female".ToSnakeCase().ToLower(), 2);

        private PetGender(string name, int value) : base(name, value) { }
    }
}
