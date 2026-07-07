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
    public class AreaLevelService : IAreaLevelService
    {
        private readonly IAreaLevelRepository _areaLevelRepository;
        private readonly ILogger<AreaLevelService> _logger;


        public AreaLevelService(IAreaLevelRepository areaLevelRepository, ILogger<AreaLevelService> logger)
        {
            _areaLevelRepository = areaLevelRepository;
            _logger = logger;

        }

        /// <summary>Creates a new area level after normalizing and validating input.</summary>
        public async Task CreateAsync(AreaLevelInputModel areaLevelInputModel)
        {
            try
            {
                areaLevelInputModel.Normalize();

                ValidationResult validationResult = new AreaLevelValidator().Validate(areaLevelInputModel);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }

                await _areaLevelRepository.CreateAsync(areaLevelInputModel.OrganizationId, areaLevelInputModel.AreaLevelName, areaLevelInputModel.AreaLevelDescription, areaLevelInputModel.SortOrder, areaLevelInputModel.CreatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }

        /// <summary>Gets all area levels for an organization.</summary>
        public async Task<IEnumerable<AreaLevelViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {

            try
            {
                IEnumerable<AreaLevel> areaLevels = await _areaLevelRepository.GetAllAsync(organizationId, rows_page, page_number);
                if (areaLevels == null) throw new ResponseExceptionFactory(Exceptions.AreaLevelNotFound);


                return areaLevels.Select(areaLevel => new AreaLevelViewModel
                {
                    Id = areaLevel.AreaLevelId,
                    OrganizationId = areaLevel.OrganizationId,
                    OrganizationName = areaLevel.OrganizationName,
                    AreaLevelName = areaLevel.AreaLevelName,
                    TotalRecords = areaLevel.TotalRecords,
                    AreaLevelDescription = areaLevel.AreaLevelDescription,
                    SortOrder = areaLevel.SortOrder,
                });

            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }





        /// <summary>Gets a area level by its identifier.</summary>
        public async Task<AreaLevelViewModel> GetByIdAsync(int id)
        {

            try
            {
                AreaLevel? areaLevel = await _areaLevelRepository.GetByIdAsync(id);

                return new AreaLevelViewModel
                {
                    Id = areaLevel.AreaLevelId,
                    OrganizationId = areaLevel.OrganizationId,
                    AreaLevelName = areaLevel.AreaLevelName,
                    AreaLevelDescription = areaLevel.AreaLevelDescription,
                    SortOrder = areaLevel.SortOrder
                };

            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }

        /// <summary>Updates an existing area level.</summary>
        public async Task UpdateAsync(int id, AreaLevelInputModel areaLevelInputModel)
        {
            try
            {
                areaLevelInputModel.Normalize();

                ValidationResult validationResult = new AreaLevelValidator().Validate(areaLevelInputModel);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }

                AreaLevel? existing = await _areaLevelRepository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.AreaLevelNotFound);

                await _areaLevelRepository.UpdateAsync(id, areaLevelInputModel.OrganizationId, areaLevelInputModel.AreaLevelName, areaLevelInputModel.AreaLevelDescription, areaLevelInputModel.SortOrder, areaLevelInputModel.UpdatedBy);
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
                AreaLevel? areaLevel = await _areaLevelRepository.GetByIdAsync(id);
                if (areaLevel == null)
                    throw new ResponseExceptionFactory(Exceptions.AreaLevelNotFound);

                await _areaLevelRepository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }

    }
}
