using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync(int organizationId, int rows_page, int page_number);
        Task<User?> GetByIdAsync(long id);
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByUsernameAsync(string username);
        Task<long> CreateAsync(int organizationId, string username, string email, string password, int createdBy);
        Task UpdateAsync(long id, string username, string email, int updatedBy);
        Task UpdatePasswordAsync(long id, string password, int updatedBy);
        Task SoftDeleteAsync(long id, int updatedBy);
        Task AssignUserRolesAsync(long userId, int organizationId, List<long> roleIds, int updatedBy);
    }
}
