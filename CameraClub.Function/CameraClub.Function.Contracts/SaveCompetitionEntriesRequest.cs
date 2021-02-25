using System.Collections.Generic;

namespace CameraClub.Function.Contracts
{
    public class SaveCompetitionEntriesRequest
    {
        public int CompetitionId { get; set; }

        public List<Photographer> Photographers { get; set; }

        public List<Photo> Photos { get; set; }
    }
}