using FluentValidation.Results;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class VacancyService : IVacancyService
    {
        private readonly IVacancyRepository _repository;
        private readonly ILogger<VacancyService> _logger;

        public VacancyService(IVacancyRepository repository, ILogger<VacancyService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<VacancyViewModel> CreateAsync(VacancyInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new VacancyValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                int newId = await _repository.CreateAsync(input.OrganizationId, input.ProfileId,
                    input.CriticalityLevelId, input.VacancyStatusId, input.VacancyTitle,
                    input.ClientName, input.ProjectName, input.VacancyLocation,
                    input.PositionCount, input.SalaryRangeMin, input.SalaryRangeMax,
                    input.RequestDate, input.DeadlineDate,
                    input.ModalityId, input.ContractTypeId, input.CurrencyId, input.PayFrequencyId, input.Notes,
                    input.CreatedBy);
                return new VacancyViewModel
                {
                    Id = newId,
                    OrganizationId = input.OrganizationId,
                    ProfileId = input.ProfileId,
                    CriticalityLevelId = input.CriticalityLevelId,
                    VacancyStatusId = input.VacancyStatusId,
                    VacancyTitle = input.VacancyTitle,
                    ClientName = input.ClientName,
                    ProjectName = input.ProjectName,
                    VacancyLocation = input.VacancyLocation,
                    PositionCount = input.PositionCount,
                    SalaryRangeMin = input.SalaryRangeMin,
                    SalaryRangeMax = input.SalaryRangeMax,
                    RequestDate = input.RequestDate,
                    DeadlineDate = input.DeadlineDate,
                    ModalityId = input.ModalityId,
                    ContractTypeId = input.ContractTypeId,
                    CurrencyId = input.CurrencyId,
                    PayFrequencyId = input.PayFrequencyId,
                    Notes = input.Notes,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating vacancy");
                throw;
            }
        }

        public async Task<VacancyViewModel> UpsertAsync(VacancyInputModel input)
        {
            try
            {
                input.Normalize();
                long result = await _repository.UpsertAsync(input.VacancyId, input.OrganizationId, input.ProfileId,
                    input.CriticalityLevelId, input.VacancyStatusId, input.VacancyTitle,
                    input.ClientName, input.ProjectName, input.VacancyLocation,
                    input.PositionCount, input.SalaryRangeMin, input.SalaryRangeMax,
                    input.RequestDate, input.DeadlineDate,
                    input.ModalityId, input.ContractTypeId, input.CurrencyId, input.PayFrequencyId, input.Notes,
                    input.CreatedBy, input.CreatedBy);
                var item = await _repository.GetByIdAsync((int)result);
                if (item == null) throw new ResponseExceptionFactory(Exceptions.VacancyNotFound);
                return MapToViewModel(item);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error upserting vacancy");
                throw;
            }
        }

        public async Task<IEnumerable<VacancyViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var items = await _repository.GetAllAsync(organizationId, rows_page, page_number);
                return items.Select(MapToViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vacancies for organization {OrganizationId}", organizationId);
                throw;
            }
        }

        public async Task<VacancyViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null) return null;
                return MapToViewModel(item);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vacancy by id {VacancyId}", id);
                throw;
            }
        }

        public async Task UpdateAsync(int id, VacancyInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new VacancyValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.VacancyNotFound);
                await _repository.UpdateAsync(id, input.ProfileId, input.CriticalityLevelId,
                    input.VacancyStatusId, input.VacancyTitle, input.ClientName, input.ProjectName,
                    input.VacancyLocation, input.PositionCount, input.SalaryRangeMin,
                    input.SalaryRangeMax, input.RequestDate, input.DeadlineDate,
                    input.ModalityId, input.ContractTypeId, input.CurrencyId, input.PayFrequencyId, input.Notes,
                    input.CreatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating vacancy {VacancyId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int updatedBy)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.VacancyNotFound);
                await _repository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error deleting vacancy {VacancyId}", id);
                throw;
            }
        }

        private static VacancyViewModel MapToViewModel(Vacancy item) => new()
        {
            Id = item.Id,
            OrganizationId = item.OrganizationId,
            ProfileId = item.ProfileId,
            ProfileName = item.ProfileName,
            CriticalityLevelId = item.CriticalityLevelId,
            VacancyStatusId = item.VacancyStatusId,
            StatusName = item.StatusName,
            VacancyTitle = item.VacancyTitle,
            ClientName = item.ClientName,
            ProjectName = item.ProjectName,
            VacancyLocation = item.VacancyLocation,
            PositionCount = item.PositionCount,
            SalaryRangeMin = item.SalaryRangeMin,
            SalaryRangeMax = item.SalaryRangeMax,
            RequestDate = item.RequestDate,
            DeadlineDate = item.DeadlineDate,
            ModalityId = item.ModalityId,
            ContractTypeId = item.ContractTypeId,
            CurrencyId = item.CurrencyId,
            PayFrequencyId = item.PayFrequencyId,
            Notes = item.Notes,
            IsEnabled = item.IsEnabled,
            TotalRecords = item.TotalRecords
        };
    }
}
