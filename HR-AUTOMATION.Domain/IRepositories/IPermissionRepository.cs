using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<Permission>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<Permission?> GetByIdAsync(long id);
        Task<Permission?> GetByPermissionNameAsync(string permissionName);
        Task<long> CreateAsync(int organizationId, string permissionName, int createdBy);
        Task UpdateAsync(long id, string permissionName, int updatedBy);
        Task SoftDeleteAsync(long id, int updatedBy);
    }
}
