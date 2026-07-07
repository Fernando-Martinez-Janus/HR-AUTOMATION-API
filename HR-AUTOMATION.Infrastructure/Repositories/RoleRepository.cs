using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ISharedRepository _shared;

        public RoleRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];
            return await _shared.QueryAsync<Role>("auth.web_get_roles", parameters);
        }

        public async Task<Role?> GetByIdAsync(long id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_role_id", id)
            ];
            var result = await _shared.QueryAsync<Role>("auth.web_get_role_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<Role?> GetByRoleNameAsync(string roleName)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_role_name", roleName)
            ];
            var result = await _shared.QueryAsync<Role>("auth.web_get_role_by_name", parameters);
            return result.FirstOrDefault();
        }

        public async Task<long> CreateAsync(int organizationId, string roleName, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_role_name", roleName),
                new("@p_created_by", createdBy)
            ];
            object? result = await _shared.QueryScalarAsync("auth.web_ins_role", parameters);
            return Convert.ToInt64(result);
        }

        public async Task UpdateAsync(long id, string roleName, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_role_id", id),
                new("@p_role_name", roleName),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("auth.web_upd_role", parameters);
        }

        public async Task SoftDeleteAsync(long id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_role_id", id),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("auth.web_del_role", parameters);
        }

        public async Task AssignPermissionsAsync(long roleId, int organizationId, List<long> permissionIds, int updatedBy)
        {
            foreach (var permissionId in permissionIds)
            {
                List<KeyValuePair<string, object?>> parameters =
                [
                    new("@p_role_id", roleId),
                    new("@p_permission_id", permissionId),
                    new("@p_organization_id", organizationId),
                    new("@p_updated_by", updatedBy)
                ];

                object? result = await _shared.QueryScalarAsync("auth.web_ins_role_permission", parameters);
            }
        }
    }
}
