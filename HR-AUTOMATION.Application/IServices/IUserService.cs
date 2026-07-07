public interface IUserService
{
    Task<UserViewModel> CreateUserAsync(UserInputModel userInputModel);
    Task<IEnumerable<UserViewModel>> GetAllUsersAsync(int organizationId, int rows_page, int page_number);
    Task<UserViewModel> GetByEmailAsync(string email);
    Task<UserViewModel> GetByIdAsync(long id);
    Task<UserViewModel> GetByUsernameAsync(string username);
    Task<UserViewModel> UpdateUserAsync(long id, UserInputModel userInputModel);
    Task UpdatePasswordAsync(long id, string password, int updatedBy);
    Task<UserViewModel> UpdateUserRolesAsync(long id, List<long> rolesIds);
    Task SoftDeleteUserAsync(long id, int updatedBy);
}
