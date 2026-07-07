public interface IProfileSkillService
{
    Task<ProfileSkillViewModel> CreateAsync(ProfileSkillInputModel input);
    Task<IEnumerable<ProfileSkillViewModel>> GetAllAsync(int profileId, int rows_page, int page_number);
    Task<ProfileSkillViewModel?> GetByIdAsync(int id);
    Task UpdateAsync(int id, ProfileSkillInputModel input);
    Task DeleteAsync(int id, int updatedBy);
}
