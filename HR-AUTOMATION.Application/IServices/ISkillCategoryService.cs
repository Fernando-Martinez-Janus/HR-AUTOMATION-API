public interface ISkillCategoryService
{
    Task<SkillCategoryViewModel> CreateAsync(SkillCategoryInputModel input);
    Task<IEnumerable<SkillCategoryViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);
    Task<SkillCategoryViewModel?> GetByIdAsync(int id);
    Task UpdateAsync(int id, SkillCategoryInputModel input);
    Task DeleteAsync(int id, int updatedBy);
}
