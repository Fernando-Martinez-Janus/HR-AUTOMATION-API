using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    /// <summary>Defines the contract for Profile data access.</summary>
    public interface IProfileRepository
    {
        /// <summary>Gets a Profile by their unique identifier, including role assignments.</summary>
        Task<Profile?> GetByIdAsync(int profileId, int organizationId);

        /// <summary>Gets all enabled Profiles with their role assignments.</summary>
        Task<IEnumerable<Profile>> GetAllAsync(int organizationId, int rows_page, int page_number);

        /// <summary>Creates a new Profile in the database.</summary>
        Task CreateAsync(int organizationId, int areaLevelId,
            int seniorityLevelId, string profileName,
            string profileDescription, int sortOrder, int createdBy);

        /// <summary>Updates an existing Profile.</summary>
        Task UpdateAsync(int id, int areaLevelId,
            int seniorityLevelId, int organizationId,
            string profileName, string profileDescription,
            int sortOrder, int updatedBy);

        /// <summary>Soft-deletes (disables) a Profile by identifier.</summary>
        Task SoftDeleteAsync(int profileId, int updatedBy);
    }
}
