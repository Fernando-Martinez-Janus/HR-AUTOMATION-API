using FluentValidation.Results;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _repository;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IRoleRepository repository, ILogger<RoleService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<RoleViewModel> CreateRoleAsync(RoleInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new RoleValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                long newId = await _repository.CreateAsync(input.OrganizationId, input.RoleName, input.CreatedBy);
                await _repository.AssignPermissionsAsync(newId, input.OrganizationId, input.PermissionIds, input.CreatedBy);
                return new RoleViewModel
                {
                    Id = newId,
                    RoleName = input.RoleName,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating role");
                throw;
            }
        }

        public async Task<IEnumerable<RoleViewModel>> GetAllRolesAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var items = await _repository.GetAllAsync(organizationId, rows_page, page_number);
                return items.Select(item => new RoleViewModel
                {
                    Id = item.Id,
                    RoleName = item.RoleName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving roles");
                throw;
            }
        }

        public async Task<RoleViewModel?> GetByIdAsync(long id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null) return null;
                return new RoleViewModel
                {
                    Id = item.Id,
                    RoleName = item.RoleName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role by id {RoleId}", id);
                throw;
            }
        }

        public async Task<RoleViewModel?> GetByRoleNameAsync(string roleName)
        {
            try
            {
                var item = await _repository.GetByRoleNameAsync(roleName);
                if (item == null) return null;
                return new RoleViewModel
                {
                    Id = item.Id,
                    RoleName = item.RoleName,
                    IsEnabled = item.IsEnabled
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving role by name {RoleName}", roleName);
                throw;
            }
        }

        public async Task<RoleViewModel> UpdateRoleAsync(long id, RoleInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new RoleValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.RoleNotFound);
                await _repository.UpdateAsync(id, input.RoleName, input.CreatedBy);
                await _repository.AssignPermissionsAsync(id, input.OrganizationId, input.PermissionIds, input.CreatedBy);
                return new RoleViewModel
                {
                    Id = id,
                    RoleName = input.RoleName,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating role {RoleId}", id);
                throw;
            }
        }

        public async Task<RoleViewModel> UpdateRolePermissionAsync(long id, List<long> permissionIds)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.RoleNotFound);
                await _repository.AssignPermissionsAsync(id, 0, permissionIds, existing.UpdatedBy ?? 0);
                return new RoleViewModel
                {
                    Id = existing.Id,
                    RoleName = existing.RoleName,
                    IsEnabled = existing.IsEnabled
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating role permissions {RoleId}", id);
                throw;
            }
        }

        public async Task DeleteRoleAsync(long id, int updatedBy)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.RoleNotFound);
                await _repository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error deleting role {RoleId}", id);
                throw;
            }
        }
    }
}
