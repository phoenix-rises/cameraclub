using System;

namespace CameraClub.Function.Contracts
{
    public class UpsertCompetitionRequest
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool HasDigital { get; set; }

        public bool HasPrint { get; set; }
    }
}