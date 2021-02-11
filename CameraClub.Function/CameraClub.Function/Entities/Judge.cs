using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("Judge", Schema = "contact")]
    public class Judge
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string PhoneNumber { get; set; }

        public List<CompetitionJudge> CompetitionJudge { get; set; }
    }
}