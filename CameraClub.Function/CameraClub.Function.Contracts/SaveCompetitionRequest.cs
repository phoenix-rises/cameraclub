using System;

namespace CameraClub.Function.Contracts
{
    public class SaveCompetitionRequest
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; }

        public bool HasDigital { get; set; }

        public bool HasPrint { get; set; }
    }
}