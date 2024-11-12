using UniLx.Domain.Exceptions;

namespace UniLx.Domain.Entities.AccountAgg
{
    public class Rating
    {
        private static readonly float MaxRating = 5.0f;

        public float Value { get; private set; }
        public int Count {  get; private set; }
        public DateTime? UpdatedAt { get; protected set; } = DateTime.UtcNow;

        public Rating()
        {            
            SetInitialRatingValue();
        }

        private void SetInitialRatingValue()
        {
            Value = 0;
            Count = 0;
        }

        public void UpdateRating(float value)
        {
            DomainException.ThrowIf(value < 0.0, "Rating must be greather than 0.");
            DomainException.ThrowIf(value > MaxRating, $"Rating must be lower than {MaxRating}.");

            var newRating = ((Value * Count) + value)/(Count + 1);
            Value = newRating;
            Count += 1;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
