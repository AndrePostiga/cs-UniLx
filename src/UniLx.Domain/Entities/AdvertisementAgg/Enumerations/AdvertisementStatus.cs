using Ardalis.SmartEnum;

namespace UniLx.Domain.Entities.AdvertisementAgg.Enumerations
{
    public abstract class AdvertisementStatus(string name, int value) : SmartEnum<AdvertisementStatus>(name, value)
    {
        public static readonly AdvertisementStatus Created = new CreatedStatus();
        public static readonly AdvertisementStatus PendingAnalysis = new PendingAnalysisStatus();
        public static readonly AdvertisementStatus Active = new ActiveStatus();
        public static readonly AdvertisementStatus Expired = new ExpiredStatus();
        public static readonly AdvertisementStatus Finished = new FinishedStatus();
        public static readonly AdvertisementStatus Declined = new DeclinedStatus();
        public static readonly AdvertisementStatus Reported = new ReportedStatus();

        public abstract bool CanChangeTo(AdvertisementStatus next);

        public sealed class CreatedStatus : AdvertisementStatus
        {
            public CreatedStatus() : base(nameof(Created).ToLower(), 1) { }

            public override bool CanChangeTo(AdvertisementStatus next)
                => next == PendingAnalysis || next == Active;
        }

        public sealed class PendingAnalysisStatus : AdvertisementStatus
        {
            public PendingAnalysisStatus() : base(nameof(PendingAnalysis).ToLower(), 2) { }

            public override bool CanChangeTo(AdvertisementStatus next)
                => next == Active || next == Declined || next == Reported;
        }

        public sealed class ActiveStatus : AdvertisementStatus
        {
            public ActiveStatus() : base(nameof(Active).ToLower(), 3) { }

            public override bool CanChangeTo(AdvertisementStatus next)
                => next == Expired || next == Finished || next == Reported;
        }

        public sealed class ExpiredStatus : AdvertisementStatus
        {
            public ExpiredStatus() : base(nameof(Expired).ToLower(), 4) { }

            public override bool CanChangeTo(AdvertisementStatus next)
                => false;
        }

        public sealed class FinishedStatus : AdvertisementStatus
        {
            public FinishedStatus() : base(nameof(Finished).ToLower(), 5) { }

            public override bool CanChangeTo(AdvertisementStatus next)
                => false;
        }

        public sealed class DeclinedStatus : AdvertisementStatus
        {
            public DeclinedStatus() : base(nameof(Declined).ToLower(), 6) { }

            public override bool CanChangeTo(AdvertisementStatus next)
                => false;
        }

        public sealed class ReportedStatus : AdvertisementStatus
        {
            public ReportedStatus() : base(nameof(Reported).ToLower(), 7) { }

            public override bool CanChangeTo(AdvertisementStatus next)
                => false;
        }
    }
}
