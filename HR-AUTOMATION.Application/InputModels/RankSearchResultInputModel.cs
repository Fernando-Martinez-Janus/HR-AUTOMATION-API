namespace HR_AUTOMATION.Application.InputModels
{
    public class RankSearchResultInputModel
    {
        public int Raking { get; set; }
        public string? Comments { get; set; }

        public void Normalize()
        {
            Comments = Comments?.Trim();
        }
    }
}