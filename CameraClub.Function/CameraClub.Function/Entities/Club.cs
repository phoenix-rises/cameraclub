using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("Club", Schema = "competition")]
    public class Club
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ClubAssocationNumber { get; set; }
    }
}