using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface IAreaLevelRepository
    {

        /// <summary>Gets all area levels for an organization.</summary>
        Task<IEnumerable<AreaLevel>> GetAllAsync(int organizationId, int rows_page, int page_number);

        /// <summary>Gets a area level by its identifier.</summary>
        Task<AreaLevel?> GetByIdAsync(int areaLevelId);

        /// <summary>Creates a new area level.</summary>
        Task CreateAsync(int organizationId, string areaLevelName, string areaLevelDescription, int sortOrder, int createdBy);

        /// <summary>Updates an existing area level.</summary>
        Task UpdateAsync(int id, int organizationId, string areaLevelName, string areaLevelDescription, int sortOrder, int updatedBy);

        /// <summary>Soft-deletes (disables) a area level.</summary>
        Task SoftDeleteAsync(int areaLevelId, int updatedBy);
    }
}
