namespace CameraClub.Function.Contracts
{
    public class SaveCategoryRequest
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public bool IsDigital { get; set; }
    }
}