using FluentValidation.Results;
using HR_AUTOMATION.Application.Validators;
using HR_AUTOMATION.Domain.IRepositories;
using Microsoft.Extensions.Logging;
using Shared.Kernel.Responses;
using Shared.Kernel.Utils.Enums;

namespace HR_AUTOMATION.Application.Services
{
    public class ProfileSkillService : IProfileSkillService
    {
        private readonly IProfileSkillRepository _repository;
        private readonly ILogger<ProfileSkillService> _logger;

        public ProfileSkillService(IProfileSkillRepository repository, ILogger<ProfileSkillService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ProfileSkillViewModel> CreateAsync(ProfileSkillInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new ProfileSkillValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                int newId = await _repository.CreateAsync(input.ProfileId, input.SkillId, input.SkillLevelId, input.SkillWeight, input.CreatedBy);
                return new ProfileSkillViewModel
                {
                    Id = newId,
                    ProfileId = input.ProfileId,
                    SkillId = input.SkillId,
                    SkillLevelId = input.SkillLevelId,
                    SkillWeight = input.SkillWeight,
                    SkillName = string.Empty,
                    LevelName = string.Empty,
                    CategoryName = string.Empty,
                    IsEnabled = true
                };
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error creating profile skill");
                throw;
            }
        }

        public async Task<IEnumerable<ProfileSkillViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            try
            {
                var items = await _repository.GetAllAsync(organizationId, rows_page, page_number);
                return items.Select(item => new ProfileSkillViewModel
                {
                    Id = item.Id,
                    ProfileId = item.ProfileId,
                    SkillId = item.SkillId,
                    SkillName = item.SkillName,
                    SkillLevelId = item.SkillLevelId,
                    LevelName = item.LevelName,
                    CategoryName = item.CategoryName,
                    SkillWeight = item.SkillWeight,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile skills for profile {organizationId}", organizationId);
                throw;
            }
        }

        public async Task<ProfileSkillViewModel?> GetByIdAsync(int id)
        {
            try
            {
                var item = await _repository.GetByIdAsync(id);
                if (item == null) return null;
                return new ProfileSkillViewModel
                {
                    Id = item.Id,
                    ProfileId = item.ProfileId,
                    SkillId = item.SkillId,
                    SkillName = item.SkillName,
                    SkillLevelId = item.SkillLevelId,
                    LevelName = item.LevelName,
                    CategoryName = item.CategoryName,
                    SkillWeight = item.SkillWeight,
                    IsEnabled = item.IsEnabled,
                    TotalRecords = item.TotalRecords
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profile skill by id {ProfileSkillId}", id);
                throw;
            }
        }

        public async Task UpdateAsync(int id, ProfileSkillInputModel input)
        {
            try
            {
                input.Normalize();
                ValidationResult validationResult = new ProfileSkillValidator().Validate(input);
                if (!validationResult.IsValid)
                {
                    ValidationFailure validationError = validationResult.Errors.First();
                    Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                    throw new ResponseExceptionFactory(currentException);
                }
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.ProfileSkillNotFound);
                await _repository.UpdateAsync(id, input.SkillId, input.SkillLevelId, input.SkillWeight, input.CreatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error updating profile skill {ProfileSkillId}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id, int updatedBy)
        {
            try
            {
                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                    throw new ResponseExceptionFactory(Exceptions.ProfileSkillNotFound);
                await _repository.SoftDeleteAsync(id, updatedBy);
            }
            catch (Exception ex) when (ex is not ResponseExceptionFactory)
            {
                _logger.LogError(ex, "Error deleting profile skill {ProfileSkillId}", id);
                throw;
            }
        }
    }
}
