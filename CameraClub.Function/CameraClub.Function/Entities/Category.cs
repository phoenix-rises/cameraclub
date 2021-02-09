using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("Category", Schema = "competition")]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDigital { get; set; }
    }
}