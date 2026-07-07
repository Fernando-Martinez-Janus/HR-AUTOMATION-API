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
    public class SkillLevelService : ISkillLevelService
    {
        private readonly ISkillLevelRepository _skillLevelRepository;
        private readonly ILogger<SkillLevelService> _logger;


        public SkillLevelService(ISkillLevelRepository skillLevelRepository, ILogger<SkillLevelService> logger)
        {
            _skillLevelRepository = skillLevelRepository;
            _logger = logger;

        }

        /// <summary>Creates a new skill level after normalizing and validating input.</summary>
        public async Task CreateAsync(SkillLevelInputModel skillLevelInputModel)
        {
            skillLevelInputModel.Normalize();

            ValidationResult validationResult = new SkillLevelValidator().Validate(skillLevelInputModel);
            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                throw new ResponseExceptionFactory(currentException);
            }

            await _skillLevelRepository.CreateAsync(skillLevelInputModel.OrganizationId, skillLevelInputModel.SkillLevelName, skillLevelInputModel.SkillLevelDescription, skillLevelInputModel.SortOrder, skillLevelInputModel.CreatedBy);
        }

        /// <summary>Gets all skill levels for an organization.</summary>
        public async Task<IEnumerable<SkillLevelViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            IEnumerable<SkillLevel> skillLevels = await _skillLevelRepository.GetAllAsync(organizationId, rows_page, page_number);
            if (skillLevels == null) throw new ResponseExceptionFactory(Exceptions.SkillLevelNotFound);


            return skillLevels.Select(skillLevel => new SkillLevelViewModel
            {
                Id = skillLevel.SkillLevelId,
                OrganizationId = skillLevel.OrganizationId,
                OrganizationName = skillLevel.OrganizationName,
                SkillLevelName = skillLevel.SkillLevelName,
                TotalRecords = skillLevel.TotalRecords,
                SkillLevelDescription = skillLevel.SkillLevelDescription,
                SortOrder = skillLevel.SortOrder,
            });
        }





        /// <summary>Gets a skill level by its identifier.</summary>
        public async Task<SkillLevelViewModel> GetByIdAsync(int id)
        {
            SkillLevel? skillLevel = await _skillLevelRepository.GetByIdAsync(id);

            return new SkillLevelViewModel
            {
                Id = skillLevel.SkillLevelId,
                OrganizationId = skillLevel.OrganizationId,
                SkillLevelName = skillLevel.SkillLevelName,
                SkillLevelDescription = skillLevel.SkillLevelDescription,
                SortOrder = skillLevel.SortOrder
            };
        }

        /// <summary>Updates an existing skill level.</summary>
        public async Task UpdateAsync(int id, SkillLevelInputModel skillLevelInputModel)
        {
            skillLevelInputModel.Normalize();

            ValidationResult validationResult = new SkillLevelValidator().Validate(skillLevelInputModel);
            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                throw new ResponseExceptionFactory(currentException);
            }

            SkillLevel? existing = await _skillLevelRepository.GetByIdAsync(id);
            if (existing == null)
                throw new ResponseExceptionFactory(Exceptions.SkillLevelNotFound);

            await _skillLevelRepository.UpdateAsync(id, skillLevelInputModel.OrganizationId, skillLevelInputModel.SkillLevelName, skillLevelInputModel.SkillLevelDescription, skillLevelInputModel.SortOrder, skillLevelInputModel.CreatedBy);
        }



        /// <summary>Soft-deletes a skill level.</summary>
        public async Task SoftDeleteAsync(int id, int updatedBy)
        {
            SkillLevel? skillLevel = await _skillLevelRepository.GetByIdAsync(id);
            if (skillLevel == null)
                throw new ResponseExceptionFactory(Exceptions.SkillLevelNotFound);

            await _skillLevelRepository.SoftDeleteAsync(id, updatedBy);
        }

    }
}
