using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CameraClub.Function.Entities
{
    [Table("Photo", Schema = "competition")]
    public class Photo
    {
        public int Id { get; set; }

        public int CompetitionId { get; set; }

        public int PhotographerId { get; set; }

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public Guid StorageId { get; set; }

        public Photographer Photographer { get; set; }

        public Category Category { get; set; }

        public List<PhotoScore> PhotoScores { get; set; }
    }
}