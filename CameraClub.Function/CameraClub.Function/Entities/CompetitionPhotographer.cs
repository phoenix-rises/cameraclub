using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("CompetitionPhotographer", Schema = "competition")]
    public class CompetitionPhotographer
    {
        public int CompetitionId { get; set; }

        public int PhotographerId { get; set; }

        public Competition Competition { get; set; }

        public Photographer Photographer { get; set; }
    }
}