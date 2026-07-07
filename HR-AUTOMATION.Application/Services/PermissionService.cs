using FluentValidation.Results;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionRepository _repository;
        private readonly ILogger<PermissionService> _logger;

        public PermissionService(IPermissionRepository repository, ILogger<PermissionService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<PermissionViewModel> CreatePermissionAsync(PermissionInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new PermissionValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                long newId = await _repository.CreateAsync(input.OrganizationId, input.PermissionName, input.CreatedBy);
                return new PermissionViewModel
                {
                    Id = newId,
                    OrganizationId = input.OrganizationId,
                    PermissionName = input.PermissionName,
                    IsEnabled = input.Enabled
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating permission");
                throw;
            }
        }

        public async Task<IEnumerable<PermissionViewModel>> GetAllPermissionsAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var items = await _repository.GetAllAsync(organizationId, rows_page, page_number);
                return items.Select(item => new PermissionViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    PermissionName = item.PermissionName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving permissions for organization {OrganizationId}", organizationId);
                throw;
            }
        }

        public async Task<PermissionViewModel?> GetByIdAsync(long id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null) return null;
                return new PermissionViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    PermissionName = item.PermissionName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving permission by id {PermissionId}", id);
                throw;
            }
        }

        public async Task<PermissionViewModel?> GetByPermissionNameAsync(string permissionName)
        {
            try
            {
                var item = await _repository.GetByPermissionNameAsync(permissionName);
                if (item == null) return null;
                return new PermissionViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    PermissionName = item.PermissionName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving permission by name {PermissionName}", permissionName);
                throw;
            }
        }

        public async Task<PermissionViewModel> UpdatePermissionAsync(long id, PermissionInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new PermissionValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.PermissionNotFound);
                await _repository.UpdateAsync(id, input.PermissionName, input.CreatedBy);
                return new PermissionViewModel
                {
                    Id = id,
                    OrganizationId = input.OrganizationId,
                    PermissionName = input.PermissionName,
                    IsEnabled = input.Enabled
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating permission {PermissionId}", id);
                throw;
            }
        }

        public async Task DeletePermissionAsync(long id, int updatedBy)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.PermissionNotFound);
                await _repository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error deleting permission {PermissionId}", id);
                throw;
            }
        }
    }
}
