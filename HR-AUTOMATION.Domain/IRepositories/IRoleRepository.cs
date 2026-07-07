using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<Role?> GetByIdAsync(long id);
        Task<Role?> GetByRoleNameAsync(string roleName);
        Task<long> CreateAsync(int organizationId, string roleName, int createdBy);
        Task UpdateAsync(long id, string roleName, int updatedBy);
        Task SoftDeleteAsync(long id, int updatedBy);
        Task AssignPermissionsAsync(long roleId, int organizationId, List<long> permissionIds, int updatedBy);
    }
}
