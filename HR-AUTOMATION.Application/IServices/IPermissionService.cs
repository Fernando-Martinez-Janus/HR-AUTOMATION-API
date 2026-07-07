public interface IPermissionService
{
    Task<PermissionViewModel> CreatePermissionAsync(PermissionInputModel permissionInput);
    Task<IEnumerable<PermissionViewModel>> GetAllPermissionsAsync(int organizationId, int rows_page, int page_number);
    Task<PermissionViewModel?> GetByIdAsync(long id);
    Task<PermissionViewModel?> GetByPermissionNameAsync(string permissionName);
    Task<PermissionViewModel> UpdatePermissionAsync(long id, PermissionInputModel permissionInput);
    Task DeletePermissionAsync(long id, int updatedBy);
}
