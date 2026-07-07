public interface ISkillService
{
    Task<SkillViewModel> CreateAsync(SkillInputModel input);
    Task<IEnumerable<SkillViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number);
    Task<SkillViewModel?> GetByIdAsync(int id);
    Task UpdateAsync(int id, SkillInputModel input);
    Task DeleteAsync(int id, int updatedBy);
}
