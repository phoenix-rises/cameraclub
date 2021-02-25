namespace CameraClub.Function.Contracts
{
    public class Photographer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompetitionNumber { get; set; }

        public string Email { get; set; }

        public string ClubNumber { get; set; }

        public bool IsDeleted { get; set; }
    }
}