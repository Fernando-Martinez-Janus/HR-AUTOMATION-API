using FluentValidation.Results;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.IServices;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Domain.IRepositories;
using HR_AUTOMATION.Domain.Models;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class CriticalityLevelService : ICriticalityLevelService
    {
        private readonly ICriticalityLevelRepository _criticalityRepository;
        private readonly ILogger<CriticalityLevelService> _logger;


        public CriticalityLevelService(ICriticalityLevelRepository criticalityLevelRepository, ILogger<CriticalityLevelService> logger)
        {
            _criticalityRepository = criticalityLevelRepository;
            _logger = logger;

        }

        /// <summary>Creates a new Criticality level after normalizing and validating input.</summary>
        public async Task CreateAsync(CriticalityLevelInputModel criticalityLevelRepository)
        {
            criticalityLevelRepository.Normalize();

            ValidationResult validationResult = new CriticalityLevelValidator().Validate(criticalityLevelRepository);
            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                throw new ResponseExceptionFactory(currentException);
            }

            await _criticalityRepository.CreateAsync(criticalityLevelRepository.OrganizationId, criticalityLevelRepository.CriticalityLevelName, criticalityLevelRepository.CriticalityLevelDescription, criticalityLevelRepository.SortOrder, criticalityLevelRepository.CreatedBy);
        }

        /// <summary>Gets a area level by its identifier.</summary>
        public async Task<CriticalityLevelViewModel> GetByIdAsync(int id)
        {
            try
            {
                CriticalityLevel? criticalityLevel = await _criticalityRepository.GetByIdAsync(id);

                return new CriticalityLevelViewModel
                {
                    Id = criticalityLevel.CriticalityLevelId,
                    OrganizationId = criticalityLevel.OrganizationId,
                    CriticalityLevelName = criticalityLevel.CriticalityLevelName,
                    CriticalityLevelDescription = criticalityLevel.CriticalityLevelDescription,
                    SortOrder = criticalityLevel.SortOrder
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }

        /// <summary>Gets all area levels for an organization.</summary>
        public async Task<IEnumerable<CriticalityLevelViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {

            try
            {
                IEnumerable<CriticalityLevel> criticalityLevel = await _criticalityRepository.GetAllAsync(organizationId, rows_page, page_number);
                if (criticalityLevel == null) throw new ResponseExceptionFactory(Exceptions.AreaLevelNotFound);


                return criticalityLevel.Select(criticalityLevel => new CriticalityLevelViewModel
                {
                    Id = criticalityLevel.CriticalityLevelId,
                    OrganizationId = criticalityLevel.OrganizationId,
                    OrganizationName = criticalityLevel.OrganizationName,
                    CriticalityLevelName = criticalityLevel.CriticalityLevelName,
                    TotalRecords = criticalityLevel.TotalRecords,
                    CriticalityLevelDescription = criticalityLevel.CriticalityLevelDescription,
                    SortOrder = criticalityLevel.SortOrder,
                });
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }


        /// <summary>Updates an existing area level.</summary>
        public async Task UpdateAsync(int id, CriticalityLevelInputModel criticalityLevelInputModel)
        {
            try
            {
                criticalityLevelInputModel.Normalize();

                Console.WriteLine(criticalityLevelInputModel);

                ValidationResult validationResult = new CriticalityLevelValidator().Validate(criticalityLevelInputModel);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }

                CriticalityLevel? existing = await _criticalityRepository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.CriticalityLevelNotFound);

                await _criticalityRepository.UpdateAsync(id, criticalityLevelInputModel.OrganizationId, criticalityLevelInputModel.CriticalityLevelName, criticalityLevelInputModel.CriticalityLevelDescription, criticalityLevelInputModel.SortOrder, criticalityLevelInputModel.CreatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }

        /// <summary>Soft-deletes a area level.</summary>
        public async Task SoftDeleteAsync(int id, int updatedBy)
        {

            try
            {
                CriticalityLevel? criticalityLevel = await _criticalityRepository.GetByIdAsync(id);
                if (criticalityLevel == null)
                    throw new ResponseExceptionFactory(Exceptions.AreaLevelNotFound);

                await _criticalityRepository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }




    }
}
