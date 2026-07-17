namespace HR_AUTOMATION.Application.InputModels
{
    public class OrganizationInputModel
    {
        public string OrganizationName { get; set; } = null!;
        public string Slug { get; set; } = null!;

        public void Normalize()
        {
            OrganizationName = OrganizationName?.Trim()!;
            Slug = Slug?.Trim()!;
        }
    }
}