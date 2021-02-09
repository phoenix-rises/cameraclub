using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("Competition", Schema = "competition")]
    public class Competition
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool HasDigital { get; set; }

        public bool HasPrint { get; set; }

        public List<Photographer> Photographers { get; set; }
    }
}