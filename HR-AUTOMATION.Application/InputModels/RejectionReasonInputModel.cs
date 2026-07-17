namespace HR_AUTOMATION.Application.InputModels
{
    public class RejectionReasonInputModel
    {
        public int? OrganizationId { get; set; }
        public string Description { get; set; } = null!;
        public bool IsDefinitive { get; set; }

        public void Normalize()
        {
            Description = Description.Trim();
        }
    }
}
