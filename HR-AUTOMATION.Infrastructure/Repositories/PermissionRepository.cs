using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class PermissionRepository : IPermissionRepository
    {
        private readonly ISharedRepository _shared;

        public PermissionRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<Permission>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];
            return await _shared.QueryAsync<Permission>("auth.web_get_permissions", parameters);
        }

        public async Task<Permission?> GetByIdAsync(long id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_permission_id", id)
            ];
            var result = await _shared.QueryAsync<Permission>("auth.web_get_permission_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<Permission?> GetByPermissionNameAsync(string permissionName)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_permission_name", permissionName)
            ];
            var result = await _shared.QueryAsync<Permission>("auth.web_get_permission_by_name", parameters);
            return result.FirstOrDefault();
        }

        public async Task<long> CreateAsync(int organizationId, string permissionName, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_permission_name", permissionName),
                new("@p_created_by", createdBy)
            ];
            object? result = await _shared.QueryScalarAsync("auth.web_ins_permission", parameters);
            return Convert.ToInt64(result);
        }

        public async Task UpdateAsync(long id, string permissionName, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_permission_id", id),
                new("@p_permission_name", permissionName),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("auth.web_upd_permission", parameters);
        }

        public async Task SoftDeleteAsync(long id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_permission_id", id),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("auth.web_del_permission", parameters);
        }
    }
}
