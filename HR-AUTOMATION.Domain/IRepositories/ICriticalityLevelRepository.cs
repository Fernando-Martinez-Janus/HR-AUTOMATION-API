using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface ICriticalityLevelRepository
    {

        /// <summary>Gets all Criticality levels for an organization.</summary>
        Task<IEnumerable<CriticalityLevel>> GetAllAsync(int organizationId, int rows_page, int page_number);

        /// <summary>Gets a Criticality level by its identifier.</summary>
        Task<CriticalityLevel?> GetByIdAsync(int criticalityLevelId);

        /// <summary>Creates a new Criticality level.</summary>
        Task CreateAsync(int organizationId, string criticalityLevelName, string criticalityLevelDescription, int sortOrder, int createdBy);

        /// <summary>Updates an existing Criticality level.</summary>
        Task UpdateAsync(int id, int organizationId, string criticalityLevelName, string criticalityLevelDescription, int sortOrder, int updatedBy);

        /// <summary>Soft-deletes (disables) a Criticality level.</summary>
        Task SoftDeleteAsync(int criticalityLevelId, int updatedBy);
    }
}
