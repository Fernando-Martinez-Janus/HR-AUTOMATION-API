using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Shared.Kernel.IRepositories;

namespace HR_AUTOMATION.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ISharedRepository _shared;

        public UserRepository(ISharedRepository shared)
        {
            _shared = shared;
        }

        public async Task<IEnumerable<User>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@page_number", page_number),
                new("@rows_page", rows_page)
            ];
            return await _shared.QueryAsync<User>("auth.web_get_users", parameters);
        }

        public async Task<User?> GetByIdAsync(long id)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_user_id", id)
            ];
            var result = await _shared.QueryAsync<User>("auth.web_get_user_by_id", parameters);
            return result.FirstOrDefault();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_email", email)
            ];
            var result = await _shared.QueryAsync<User>("auth.web_get_user_by_email", parameters);
            return result.FirstOrDefault();
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_username", username)
            ];
            var result = await _shared.QueryAsync<User>("auth.web_get_user_by_username", parameters);
            return result.FirstOrDefault();
        }

        public async Task<long> CreateAsync(int organizationId, string username, string email, string password, int createdBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_organization_id", organizationId),
                new("@p_username", username),
                new("@p_email", email),
                new("@p_password", password),
                new("@p_created_by", createdBy)
            ];
            object? result = await _shared.QueryScalarAsync("auth.web_ins_user", parameters);
            return Convert.ToInt64(result);
        }

        public async Task UpdateAsync(long id, string username, string email, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_user_id", id),
                new("@p_username", username),
                new("@p_email", email),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("auth.web_upd_user", parameters);
        }

        public async Task UpdatePasswordAsync(long id, string password, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_user_id", id),
                new("@p_password", password),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("auth.web_upd_user_password", parameters);
        }

        public async Task SoftDeleteAsync(long id, int updatedBy)
        {
            List<KeyValuePair<string, object?>> parameters =
            [
                new("@p_user_id", id),
                new("@p_updated_by", updatedBy)
            ];
            await _shared.ExecuteAsync("auth.web_del_user", parameters);
        }

        public async Task AssignUserRolesAsync(long userId, int organizationId, List<long> roleIds, int updatedBy)
        {
            foreach (var roleId in roleIds)
            {
                List<KeyValuePair<string, object?>> parameters =
                [
                    new("@p_organization_id", organizationId),
                    new("@p_user_id", userId),
                    new("@p_role_id", roleId),
                    new("@p_created_by", updatedBy)
                ];
                await _shared.ExecuteAsync("auth.web_ins_user_role", parameters);
            }
        }
    }
}
