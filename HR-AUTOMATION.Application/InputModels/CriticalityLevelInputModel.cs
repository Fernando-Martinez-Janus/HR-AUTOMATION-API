namespace HR_AUTOMATION.Application.InputModels
{
    public class CriticalityLevelInputModel
    {
        public int? OrganizationId { get; set; }
        public string LevelName { get; set; } = null!;
        public string LevelDescription { get; set; } = null!;

        public void Normalize()
        {
            LevelName = LevelName.Trim();
            LevelDescription = LevelDescription.Trim();
        }
    }
}
