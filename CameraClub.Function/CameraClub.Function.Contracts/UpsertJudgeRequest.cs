namespace CameraClub.Function.Contracts
{
    public class UpsertJudgeRequest
    {
        public int? Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Bio { get; set; }

        public string PhoneNumber { get; set; }
    }
}