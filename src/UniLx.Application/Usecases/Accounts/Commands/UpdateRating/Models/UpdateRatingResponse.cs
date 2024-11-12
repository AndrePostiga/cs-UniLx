namespace UniLx.Application.Usecases.Accounts.Commands.UpdateRating.Models
{
    internal class UpdateRatingResponse
    {
        public float Rating { get; set; }
        public int Votes { get; set; }

        public UpdateRatingResponse(float rating, int votes)
        {
            Rating = rating;
            Votes = votes;
        }
    }
}
