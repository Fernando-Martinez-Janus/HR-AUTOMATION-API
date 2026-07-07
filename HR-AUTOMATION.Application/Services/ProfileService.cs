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
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly ILogger<ProfileService> _logger;


        public ProfileService(IProfileRepository profileRepository, ILogger<ProfileService> logger)
        {
            _profileRepository = profileRepository;
            _logger = logger;

        }

        /// <summary>Creates a new area level after normalizing and validating input.</summary>
        public async Task CreateAsync(ProfileInputModel profileInputModel)
        {
            profileInputModel.Normalize();

            ValidationResult validationResult = new ProfileValidator().Validate(profileInputModel);
            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                throw new ResponseExceptionFactory(currentException);
            }

            await _profileRepository.CreateAsync(
                profileInputModel.OrganizationId,
                profileInputModel.AreaLevelId,
                profileInputModel.SeniorityLevelId,
                profileInputModel.ProfileName,
                profileInputModel.ProfileDescription,
                profileInputModel.SortOrder,
                profileInputModel.CreatedBy
            );
        }

        /// <summary>Gets all area levels for an organization.</summary>
        public async Task<IEnumerable<ProfileViewModel>> GetAllAsync(int organizationId, int rows_page, int page_number)
        {
            IEnumerable<Profile> profile = await _profileRepository.GetAllAsync(organizationId, rows_page, page_number);
            if (profile == null) throw new ResponseExceptionFactory(Exceptions.ProfileNotFound);


            return profile.Select(profile => new ProfileViewModel
            {
                ProfileId = profile.ProfileId,
                OrganizationId = profile.OrganizationId,
                AreaLevelId = profile.AreaLevelId,
                AreaLevelName = profile.AreaLevelName,
                SeniorityLevelId = profile.SeniorityLevelId,
                SeniorityLevelName = profile.SeniorityLevelName,
                ProfileName = profile.ProfileName,
                ProfileDescription = profile.ProfileDescription,
                SortOrder = profile.SortOrder
            });
        }


        /// <summary>Gets a area level by its identifier.</summary>
        public async Task<ProfileViewModel> GetByIdAsync(int id, int organizationId)
        {
            Profile? profile = await _profileRepository.GetByIdAsync(id, organizationId);

            return new ProfileViewModel
            {
                ProfileId = profile.ProfileId,
                OrganizationId = profile.OrganizationId,
                AreaLevelId = profile.AreaLevelId,
                AreaLevelName = profile.AreaLevelName,
                SeniorityLevelId = profile.SeniorityLevelId,
                SeniorityLevelName = profile.SeniorityLevelName,
                ProfileName = profile.ProfileName,
                ProfileDescription = profile.ProfileDescription,
            };
        }


        /// <summary>Updates an existing area level.</summary>
        public async Task UpdateAsync(int id, ProfileInputModel profileInputModel)
        {
            profileInputModel.Normalize();

            ValidationResult validationResult = new ProfileValidator().Validate(profileInputModel);
            if (!validationResult.IsValid)
            {
                ValidationFailure validationError = validationResult.Errors.First();
                Exceptions currentException = Enum.Parse<Exceptions>(validationError.ErrorCode);
                throw new ResponseExceptionFactory(currentException);
            }

            Profile? existing = await _profileRepository.GetByIdAsync(id, profileInputModel.OrganizationId);
            if (existing == null)
                throw new ResponseExceptionFactory(Exceptions.ProfileNotFound);

            await _profileRepository.UpdateAsync(
                id,
                profileInputModel.AreaLevelId,
                profileInputModel.SeniorityLevelId,
                profileInputModel.OrganizationId,
                profileInputModel.ProfileName,
                profileInputModel.ProfileDescription,
                profileInputModel.SortOrder,
                profileInputModel.UpdatedBy
                );
        }



        /// <summary>Soft-deletes a area level.</summary>
        public async Task SoftDeleteAsync(int id, int organizationId, int updatedBy)
        {
            Profile? profile = await _profileRepository.GetByIdAsync(id, organizationId);
            if (profile == null)
                throw new ResponseExceptionFactory(Exceptions.ProfileNotFound);

            await _profileRepository.SoftDeleteAsync(id, updatedBy);
        }


    }
}
