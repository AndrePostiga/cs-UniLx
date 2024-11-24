using Ardalis.SmartEnum;
using UniLx.Shared.LibExtensions;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public class WorkLocationType : SmartEnum<WorkLocationType>
    {
        public static readonly WorkLocationType Remote = new WorkLocationType("Remote".ToSnakeCase().ToLower(), 1);
        public static readonly WorkLocationType OnSite = new WorkLocationType("OnSite".ToSnakeCase().ToLower(), 2);
        public static readonly WorkLocationType Hybrid = new WorkLocationType("Hybrid".ToSnakeCase().ToLower(), 3);

        private WorkLocationType(string name, int value) : base(name, value) { }
    }
}
