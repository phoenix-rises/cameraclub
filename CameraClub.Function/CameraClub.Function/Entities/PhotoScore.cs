using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("PhotoScore", Schema = "competition")]
    public class PhotoScore
    {
        public int PhotoId { get; set; }

        public int JudgeId { get; set; }

        public int Round { get; set; }

        public string Score { get; set; }

        public string Rank { get; set; }

        public Judge Judge { get; set; }
    }
}