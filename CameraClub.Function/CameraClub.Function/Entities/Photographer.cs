using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("Photographer", Schema = "contact")]
    public class Photographer
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string CompetitionNumber { get; set; }

        public string Email { get; set; }

        public string ClubNumber { get; set; }
    }
}