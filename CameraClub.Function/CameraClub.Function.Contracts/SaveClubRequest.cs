namespace CameraClub.Function.Contracts
{
    public class SaveClubRequest
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public string ContactName { get; set; }

        public string ContactEmail { get; set; }

        public string ClubAssociationNumber { get; set; }
    }
}