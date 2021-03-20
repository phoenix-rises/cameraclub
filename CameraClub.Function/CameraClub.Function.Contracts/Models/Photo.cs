using System;

namespace CameraClub.Function.Contracts
{
    public class Photo
    {
        public int Id { get; set; }

        public int CompetitionId { get; set; }

        public int PhotographerId { get; set; }

        public string Title { get; set; }

        public int CategoryId { get; set; }

        public string FileName { get; set; }

        public Guid? StorageId { get; set; }

        public bool IsDeleted { get; set; }
    }
}