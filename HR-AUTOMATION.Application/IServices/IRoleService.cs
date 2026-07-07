public interface IRoleService
{
    Task<RoleViewModel> CreateRoleAsync(RoleInputModel roleInput);
    Task<IEnumerable<RoleViewModel>> GetAllRolesAsync(int organizationId, int rows_page, int page_number);
    Task<RoleViewModel?> GetByIdAsync(long id);
    Task<RoleViewModel?> GetByRoleNameAsync(string roleName);
    Task<RoleViewModel> UpdateRoleAsync(long id, RoleInputModel roleInput);
    Task<RoleViewModel> UpdateRolePermissionAsync(long id, List<long> permissionIds);
    Task DeleteRoleAsync(long id, int updatedBy);
}
