using FluentValidation.Results;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class VacancyStatusService : IVacancyStatusService
    {
        private readonly IVacancyStatusRepository _repository;
        private readonly ILogger<VacancyStatusService> _logger;

        public VacancyStatusService(IVacancyStatusRepository repository, ILogger<VacancyStatusService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<VacancyStatusViewModel> CreateAsync(VacancyStatusInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new VacancyStatusValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                int newId = await _repository.CreateAsync(input.OrganizationId, input.StatusName, input.StatusDescription, input.SortOrder, input.CreatedBy);
                return new VacancyStatusViewModel
                {
                    Id = newId,
                    OrganizationId = input.OrganizationId,
                    StatusName = input.StatusName,
                    StatusDescription = input.StatusDescription,
                    SortOrder = input.SortOrder,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating vacancy status");
                throw;
            }
        }

        public async Task<IEnumerable<VacancyStatusViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var items = await _repository.GetAllAsync(organizationId, rows_page, page_number);
                return items.Select(item => new VacancyStatusViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    StatusName = item.StatusName,
                    StatusDescription = item.StatusDescription,
                    SortOrder = item.SortOrder,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vacancy statuses for organization {OrganizationId}", organizationId);
                throw;
            }
        }

        public async Task<VacancyStatusViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null) return null;
                return new VacancyStatusViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    StatusName = item.StatusName,
                    StatusDescription = item.StatusDescription,
                    SortOrder = item.SortOrder,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vacancy status by id {VacancyStatusId}", id);
                throw;
            }
        }

        public async Task UpdateAsync(int id, VacancyStatusInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new VacancyStatusValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.VacancyStatusNotFound);
                await _repository.UpdateAsync(id, input.StatusName, input.StatusDescription, input.SortOrder, input.CreatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating vacancy status {VacancyStatusId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int updatedBy)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.VacancyStatusNotFound);
                await _repository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error deleting vacancy status {VacancyStatusId}", id);
                throw;
            }
        }
    }
}
