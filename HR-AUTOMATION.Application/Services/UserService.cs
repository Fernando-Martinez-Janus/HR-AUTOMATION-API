using FluentValidation.Results;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IPasswordService _passwordService;

        public UserService(IUserRepository userRepository, ILogger<UserService> logger, IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
            _logger = logger;
        }

        public async Task<UserViewModel> CreateUserAsync(UserInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new UserValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }

                string passwordHash = await _passwordService.HashPassword(input.Password);
                long newId = await _userRepository.CreateAsync(input.OrganizationId, input.Username, input.Email, passwordHash, input.CreatedBy);

                if (input.RolesIds != null && input.RolesIds.Any())
                {
                    await _userRepository.AssignUserRolesAsync(newId, input.OrganizationId, input.RolesIds, input.CreatedBy);
                }

                return new UserViewModel
                {
                    Id = newId,
                    OrganizationId = input.OrganizationId,
                    Username = input.Username,
                    Email = input.Email,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        public async Task<UserViewModel> GetByIdAsync(long id)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(id);
                if (user == null) throw new ResponseExceptionFactory(Exceptions.UserNotFound);

                return new UserViewModel
                {
                    Id = user.Id,
                    OrganizationId = user.OrganizationId,
                    OrganizationName = user.OrganizationName,
                    Username = user.Username,
                    Email = user.Email,
                    IsEnabled = user.IsEnabled,
                    TotalRecords = user.TotalRecords
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error retrieving user by id {UserId}", id);
                throw;
            }
        }

        public async Task<UserViewModel> GetByUsernameAsync(string username)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(username);
                if (user == null) throw new ResponseExceptionFactory(Exceptions.UserNotFound);

                return new UserViewModel
                {
                    Id = user.Id,
                    OrganizationId = user.OrganizationId,
                    Username = user.Username,
                    Email = user.Email
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error retrieving user by username {Username}", username);
                throw;
            }
        }

        public async Task<IEnumerable<UserViewModel>> GetAllUsersAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var users = await _userRepository.GetAllAsync(organizationId, rows_page, page_number);
                return users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    OrganizationId = user.OrganizationId,
                    OrganizationName = user.OrganizationName,
                    Username = user.Username,
                    Email = user.Email,
                    IsEnabled = user.IsEnabled,
                    TotalRecords = user.TotalRecords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users for organization {OrganizationId}", organizationId);
                throw;
            }
        }

        public async Task<UserViewModel> GetByEmailAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null) throw new ResponseExceptionFactory(Exceptions.UserNotFound);

                return new UserViewModel
                {
                    Id = user.Id,
                    OrganizationId = user.OrganizationId,
                    Username = user.Username,
                    Email = user.Email
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error retrieving user by email {Email}", email);
                throw;
            }
        }

        public async Task<UserViewModel> UpdateUserAsync(long id, UserInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new UserValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }

                var existing = await _userRepository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.UserNotFound);

                await _userRepository.UpdateAsync(id, input.Username, input.Email, input.CreatedBy);

                if (!string.IsNullOrEmpty(input.Password))
                {
                    string newHash = await _passwordService.HashPassword(input.Password);
                    await _userRepository.UpdatePasswordAsync(id, newHash, input.CreatedBy);
                }

                return new UserViewModel
                {
                    Id = id,
                    OrganizationId = existing.OrganizationId,
                    Username = input.Username,
                    Email = input.Email,
                    IsEnabled = existing.IsEnabled
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating user {UserId}", id);
                throw;
            }
        }

        public async Task UpdatePasswordAsync(long id, string password, int updatedBy)
        {
            try
            {
                var existing = await _userRepository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.UserNotFound);

                string newHash = await _passwordService.HashPassword(password);
                await _userRepository.UpdatePasswordAsync(id, newHash, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating password for user {UserId}", id);
                throw;
            }
        }

        public async Task<UserViewModel> UpdateUserRolesAsync(long id, List<long> rolesIds)
        {
            try
            {
                var existing = await _userRepository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.UserNotFound);

                await _userRepository.AssignUserRolesAsync(id, existing.OrganizationId, rolesIds, existing.UpdatedBy ?? 0);

                return new UserViewModel
                {
                    Id = existing.Id,
                    OrganizationId = existing.OrganizationId,
                    Username = existing.Username,
                    Email = existing.Email,
                    IsEnabled = existing.IsEnabled
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating roles for user {UserId}", id);
                throw;
            }
        }

        public async Task SoftDeleteUserAsync(long id, int updatedBy)
        {
            try
            {
                var existing = await _userRepository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.UserNotFound);

                await _userRepository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error soft-deleting user {UserId}", id);
                throw;
            }
        }
    }
}
