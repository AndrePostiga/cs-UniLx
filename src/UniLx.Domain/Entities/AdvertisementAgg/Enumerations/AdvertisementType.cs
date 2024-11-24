using Ardalis.SmartEnum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public sealed class AdvertisementType : SmartEnum<AdvertisementType>
    {
        // Animais de Estimação
        public static readonly AdvertisementType Pets = new AdvertisementType(nameof(Pets).ToSnakeCase().ToLower(), 1);

        // Beleza
        public static readonly AdvertisementType Beauty = new AdvertisementType(nameof(Beauty).ToSnakeCase().ToLower(), 2);

        // Eletrônicos
        public static readonly AdvertisementType Electronics = new AdvertisementType(nameof(Electronics).ToSnakeCase().ToLower(), 3);

        // Eventos
        public static readonly AdvertisementType Events = new AdvertisementType(nameof(Events).ToLower().ToSnakeCase(), 4);

        // Moda
        public static readonly AdvertisementType Fashion = new AdvertisementType(nameof(Fashion).ToLower().ToSnakeCase(), 5);

        // Imóveis
        public static readonly AdvertisementType RealEstate = new AdvertisementType(nameof(RealEstate).ToLower().ToSnakeCase(), 6);

        // Serviços
        public static readonly AdvertisementType Services = new AdvertisementType(nameof(Services).ToLower().ToSnakeCase(), 7);

        // Veículos
        public static readonly AdvertisementType Vehicles = new AdvertisementType(nameof(Vehicles).ToLower().ToSnakeCase(), 8);

        // Oportunidades de emprego
        public static readonly AdvertisementType JobOpportunities = new AdvertisementType(nameof(JobOpportunities).ToSnakeCase().ToLower(), 9);

        // Brinquedos
        public static readonly AdvertisementType Toys = new AdvertisementType(nameof(Toys).ToSnakeCase().ToLower(), 10);

        // Outros
        public static readonly AdvertisementType Others = new AdvertisementType(nameof(Others).ToSnakeCase().ToLower(), 11);

        private AdvertisementType(string name, int value) : base(name, value) { }
    }
}
