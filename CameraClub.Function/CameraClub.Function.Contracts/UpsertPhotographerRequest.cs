namespace CameraClub.Function.Contracts
{
    public class UpsertPhotographerRequest
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompetitionNumber { get; set; }

        public string Email { get; set; }

        public string ClubNumber { get; set; }
    }
}