using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public sealed class AdvertisementType : SmartEnum<AdvertisementType>
    {
        // Animais de Estimação
        public static readonly AdvertisementType Pets = new AdvertisementType(nameof(Pets).ToSnakeCase(), 1);

        // Beleza
        public static readonly AdvertisementType Beauty = new AdvertisementType(nameof(Beauty).ToSnakeCase(), 2);

        // Eletrônicos
        public static readonly AdvertisementType Electronics = new AdvertisementType(nameof(Electronics).ToSnakeCase(), 3);

        // Eventos
        public static readonly AdvertisementType Events = new AdvertisementType(nameof(Events).ToSnakeCase(), 4);

        // Moda
        public static readonly AdvertisementType Fashion = new AdvertisementType(nameof(Fashion).ToSnakeCase(), 5);

        // Imóveis
        public static readonly AdvertisementType RealEstate = new AdvertisementType(nameof(RealEstate).ToSnakeCase(), 6);

        // Oportunidades de emprego
        public static readonly AdvertisementType JobOpportunities = new AdvertisementType(nameof(JobOpportunities).ToSnakeCase(), 7);

        // Outros
        public static readonly AdvertisementType Others = new AdvertisementType(nameof(Others).ToSnakeCase(), 8);

        private AdvertisementType(string name, int value) : base(name, value) { }
    }
}
