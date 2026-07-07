using FluentValidation.Results;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class SkillCategoryService : ISkillCategoryService
    {
        private readonly ISkillCategoryRepository _repository;
        private readonly ILogger<SkillCategoryService> _logger;

        public SkillCategoryService(ISkillCategoryRepository repository, ILogger<SkillCategoryService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SkillCategoryViewModel> CreateAsync(SkillCategoryInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new SkillCategoryValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                int newId = await _repository.CreateAsync(input.OrganizationId, input.CategoryName, input.CreatedBy);
                return new SkillCategoryViewModel
                {
                    Id = newId,
                    OrganizationId = input.OrganizationId,
                    CategoryName = input.CategoryName,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating skill category");
                throw;
            }
        }

        public async Task<IEnumerable<SkillCategoryViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var items = await _repository.GetAllAsync(organizationId, rows_page, page_number);
                return items.Select(item => new SkillCategoryViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    CategoryName = item.CategoryName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving skill categories for organization {OrganizationId}", organizationId);
                throw;
            }
        }

        public async Task<SkillCategoryViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null) return null;
                return new SkillCategoryViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    CategoryName = item.CategoryName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving skill category by id {SkillCategoryId}", id);
                throw;
            }
        }

        public async Task UpdateAsync(int id, SkillCategoryInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new SkillCategoryValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.SkillCategoryNotFound);
                await _repository.UpdateAsync(id, input.CategoryName, input.CreatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating skill category {SkillCategoryId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int updatedBy)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.SkillCategoryNotFound);
                await _repository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error deleting skill category {SkillCategoryId}", id);
                throw;
            }
        }
    }
}
