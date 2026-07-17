namespace HR_AUTOMATION.Application.InputModels
{
    public class WorkModalityInputModel
    {
        public int? OrganizationId { get; set; }
        public string ModalityName { get; set; } = null!;

        public void Normalize()
        {
            ModalityName = ModalityName.Trim();
        }
    }
}
