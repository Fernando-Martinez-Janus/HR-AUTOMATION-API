using FluentValidation.Results;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _repository;
        private readonly ILogger<SkillService> _logger;

        public SkillService(ISkillRepository repository, ILogger<SkillService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<SkillViewModel> CreateAsync(SkillInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new SkillValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                int newId = await _repository.CreateAsync(input.OrganizationId, input.SkillCategoryId, input.SkillName, input.CreatedBy);
                return new SkillViewModel
                {
                    Id = newId,
                    OrganizationId = input.OrganizationId,
                    SkillCategoryId = input.SkillCategoryId,
                    SkillName = input.SkillName,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating skill");
                throw;
            }
        }

        public async Task<IEnumerable<SkillViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var items = await _repository.GetAllAsync(organizationId, rows_page, page_number);
                return items.Select(item => new SkillViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    SkillCategoryId = item.SkillCategoryId,
                    SkillName = item.SkillName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving skills for organization {OrganizationId}", organizationId);
                throw;
            }
        }

        public async Task<SkillViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null) return null;
                return new SkillViewModel
                {
                    Id = item.Id,
                    OrganizationId = item.OrganizationId,
                    SkillCategoryId = item.SkillCategoryId,
                    SkillName = item.SkillName,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving skill by id {SkillId}", id);
                throw;
            }
        }

        public async Task UpdateAsync(int id, SkillInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new SkillValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.SkillNotFound);
                await _repository.UpdateAsync(id, input.SkillCategoryId, input.SkillName, input.CreatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating skill {SkillId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int updatedBy)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.SkillNotFound);
                await _repository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error deleting skill {SkillId}", id);
                throw;
            }
        }
    }
}
