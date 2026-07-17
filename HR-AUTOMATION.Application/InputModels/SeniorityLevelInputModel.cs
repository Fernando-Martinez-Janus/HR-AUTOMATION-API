namespace HR_AUTOMATION.Application.InputModels
{
    public class SeniorityLevelInputModel
    {
        public int? OrganizationId { get; set; }
        public string SeniorityName { get; set; } = null!;

        public void Normalize()
        {
            SeniorityName = SeniorityName.Trim();
        }
    }
}
