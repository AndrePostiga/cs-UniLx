using UniLx.Domain.Entities.AdvertisementAgg.Enumerations;

namespace UniLx.Tests.Domain.AdvertisementAgg.Enumerations
{
    public class AdvertisementStatusTests
    {
        [Fact]
        public void CreatedStatus_ShouldHaveCorrectNameAndValue()
        {
            // Assert
            Assert.Equal("created", AdvertisementStatus.Created.Name);
            Assert.Equal(1, AdvertisementStatus.Created.Value);
        }

        [Fact]
        public void PendingAnalysisStatus_ShouldHaveCorrectNameAndValue()
        {
            // Assert
            Assert.Equal("pending_analysis", AdvertisementStatus.PendingAnalysis.Name);
            Assert.Equal(2, AdvertisementStatus.PendingAnalysis.Value);
        }

        [Fact]
        public void ActiveStatus_ShouldHaveCorrectNameAndValue()
        {
            // Assert
            Assert.Equal("active", AdvertisementStatus.Active.Name);
            Assert.Equal(3, AdvertisementStatus.Active.Value);
        }

        [Fact]
        public void CreatedStatus_CanChangeTo_ShouldReturnTrueForValidTransitions()
        {
            // Act & Assert
            Assert.True(AdvertisementStatus.Created.CanChangeTo(AdvertisementStatus.PendingAnalysis));
            Assert.True(AdvertisementStatus.Created.CanChangeTo(AdvertisementStatus.Active));
            Assert.False(AdvertisementStatus.Created.CanChangeTo(AdvertisementStatus.Expired));
        }

        [Fact]
        public void PendingAnalysisStatus_CanChangeTo_ShouldReturnTrueForValidTransitions()
        {
            // Act & Assert
            Assert.True(AdvertisementStatus.PendingAnalysis.CanChangeTo(AdvertisementStatus.Active));
            Assert.True(AdvertisementStatus.PendingAnalysis.CanChangeTo(AdvertisementStatus.Declined));
            Assert.True(AdvertisementStatus.PendingAnalysis.CanChangeTo(AdvertisementStatus.Reported));
            Assert.False(AdvertisementStatus.PendingAnalysis.CanChangeTo(AdvertisementStatus.Expired));
        }

        [Fact]
        public void ActiveStatus_CanChangeTo_ShouldReturnTrueForValidTransitions()
        {
            // Act & Assert
            Assert.True(AdvertisementStatus.Active.CanChangeTo(AdvertisementStatus.Expired));
            Assert.True(AdvertisementStatus.Active.CanChangeTo(AdvertisementStatus.Finished));
            Assert.True(AdvertisementStatus.Active.CanChangeTo(AdvertisementStatus.Reported));
            Assert.False(AdvertisementStatus.Active.CanChangeTo(AdvertisementStatus.Declined));
        }

        [Fact]
        public void ExpiredStatus_CanChangeTo_ShouldReturnFalseForAllTransitions()
        {
            // Act & Assert
            Assert.False(AdvertisementStatus.Expired.CanChangeTo(AdvertisementStatus.Created));
            Assert.False(AdvertisementStatus.Expired.CanChangeTo(AdvertisementStatus.Active));
        }

        [Fact]
        public void ReportedStatus_CanChangeTo_ShouldReturnFalseForAllTransitions()
        {
            // Act & Assert
            Assert.False(AdvertisementStatus.Reported.CanChangeTo(AdvertisementStatus.Created));
            Assert.False(AdvertisementStatus.Reported.CanChangeTo(AdvertisementStatus.Active));
        }
    }
}