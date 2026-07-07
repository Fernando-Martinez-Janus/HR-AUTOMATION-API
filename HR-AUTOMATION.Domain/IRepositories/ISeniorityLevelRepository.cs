using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    /// <summary>Defines the contract for seniority level data access via stored procedures.</summary>
    public interface ISeniorityLevelRepository
    {
        /// <summary>Gets all active seniority levels for an organization with pagination.</summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="rows_page">Number of rows per page.</param>
        /// <param name="page_number">Page number to retrieve.</param>
        /// <returns>A collection of seniority levels.</returns>
        Task<IEnumerable<SeniorityLevel>> GetAllAsync(int organizationId, int rows_page, int page_number);

        /// <summary>Gets a seniority level by its identifier.</summary>
        /// <param name="id">The seniority level identifier.</param>
        /// <returns>The seniority level if found; otherwise null.</returns>
        Task<SeniorityLevel?> GetByIdAsync(int id);

        /// <summary>Creates a new seniority level and returns the generated identifier.</summary>
        /// <param name="organizationId">The organization identifier.</param>
        /// <param name="seniorityName">The seniority level name.</param>
        /// <param name="sortOrder">The display order (optional, auto-generated if null).</param>
        /// <param name="createdBy">The identifier of the user creating the record.</param>
        /// <returns>The newly generated seniority level identifier.</returns>
        Task<int> CreateAsync(int organizationId, string seniorityName, int? sortOrder, int createdBy);

        /// <summary>Updates an existing seniority level.</summary>
        /// <param name="id">The seniority level identifier to update.</param>
        /// <param name="seniorityName">The updated name.</param>
        /// <param name="sortOrder">The updated display order (optional, auto-generated if null).</param>
        /// <param name="updatedBy">The identifier of the user updating the record.</param>
        Task UpdateAsync(int id, string seniorityName, int? sortOrder, int updatedBy);

        /// <summary>Soft-deletes (disables) a seniority level.</summary>
        /// <param name="id">The seniority level identifier to disable.</param>
        /// <param name="updatedBy">The identifier of the user performing the operation.</param>
        Task SoftDeleteAsync(int id, int updatedBy);
    }
}
