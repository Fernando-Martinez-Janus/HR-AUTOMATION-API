using FluentValidation.Results;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    /// <summary>Handles seniority level business logic including creation, retrieval, updates, and soft delete.</summary>
    public class SeniorityLevelService : ISeniorityLevelService
    {
        private readonly ISeniorityLevelRepository _repository;
        private readonly ILogger<SeniorityLevelService> _logger;

        /// <summary>Initializes a new instance of the <see cref="SeniorityLevelService"/> class.</summary>
        public SeniorityLevelService(ISeniorityLevelRepository repository, ILogger<SeniorityLevelService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>Validates input, calls repository to create, and returns a view model with the new identifier.</summary>
        public async Task<SeniorityLevelViewModel> CreateAsync(SeniorityLevelInputModel input)
        {
            try
            {
                input.Normalize();

                ValidationResult validationResult = new SeniorityLevelValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }

                int newId = await _repository.CreateAsync(input.OrganizationId, input.SeniorityName, input.SortOrder, input.CreatedBy);

                return new SeniorityLevelViewModel
                {
                    Id = newId,
                    OrganizationId = input.OrganizationId,
                    SeniorityName = input.SeniorityName,
                    SortOrder = input.SortOrder,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating seniority level");
                throw;
            }
        }

        /// <summary>Retrieves all active seniority levels for an organization with pagination and maps them to view models.</summary>
        public async Task<IEnumerable<SeniorityLevelViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var items = await _repository.GetAllAsync(organizationId, rows_page, page_number);
                return items.Select(item => new SeniorityLevelViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    SeniorityName = item.SeniorityName,
                    OrganizationName = item.OrganizationName,
                    SortOrder = item.SortOrder,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving seniority levels for organization {OrganizationId}", organizationId);
                throw;
            }
        }

        /// <summary>Retrieves a single seniority level by identifier and maps it to a view model.</summary>
        public async Task<SeniorityLevelViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null) return null;

                return new SeniorityLevelViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    SeniorityName = item.SeniorityName,
                    OrganizationName = item.OrganizationName,
                    SortOrder = item.SortOrder,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving seniority level by id {SeniorityLevelId}", id);
                throw;
            }
        }

        /// <summary>Validates input, verifies existence, and updates the record.</summary>
        public async Task UpdateAsync(int id, SeniorityLevelInputModel input)
        {
            try
            {
                input.Normalize();

                ValidationResult validationResult = new SeniorityLevelValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }

                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.SeniorityLevelNotFound);

                await _repository.UpdateAsync(id, input.SeniorityName, input.SortOrder, input.CreatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating seniority level {SeniorityLevelId}", id);
                throw;
            }
        }

        /// <summary>Verifies existence and soft-deletes the record.</summary>
        public async Task DeleteAsync(int id, int updatedBy)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.SeniorityLevelNotFound);

                await _repository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error deleting seniority level {SeniorityLevelId}", id);
                throw;
            }
        }
    }
}
