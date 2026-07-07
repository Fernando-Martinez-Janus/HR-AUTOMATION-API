namespace HR_AUTOMATION.Application.IServices
{
    public interface IProfileService
    {

        Task<IEnumerable<ProfileViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<ProfileViewModel> GetByIdAsync(int id, int organizationId);
        Task CreateAsync(ProfileInputModel profileRecruitmentInputModel);
        Task UpdateAsync(int id, ProfileInputModel profileRecruitmentInputModel);
        Task SoftDeleteAsync(int id, int organizationId, int updatedBy);
    }
}
