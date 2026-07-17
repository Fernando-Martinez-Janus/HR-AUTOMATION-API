namespace HR_AUTOMATION.Application.InputModels
{
    public class EmploymentTypeInputModel
    {
        public int? OrganizationId { get; set; }
        public string TypeName { get; set; } = null!;

        public void Normalize()
        {
            TypeName = TypeName.Trim();
        }
    }
}
