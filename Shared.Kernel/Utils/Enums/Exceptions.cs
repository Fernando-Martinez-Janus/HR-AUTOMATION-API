namespace Shared.Kernel.Utils.Enums
{

    public enum Exceptions
    {
        #region 400 - Bad Request

        [InfoAttribute(400, "Invalid Email")]
        InvalidEmail,

        [InfoAttribute(400, "Invalid password")]
        InvalidPassword,

        [InfoAttribute(400, "Invalid Credentials")]
        InvalidCredentials,

        [InfoAttribute(400, "Username must be between 3 and 100 characters")]
        UsernameLengthInvalid,

        [InfoAttribute(400, "Role name must be between 3 and 100 characters")]
        RoleNameLengthInvalid,

        [InfoAttribute(400, "Permission name must be between 3 and 100 characters")]
        PermissionNameLengthInvalid,

        [InfoAttribute(400, "Role must have at least one permission")]
        RolePermissionsEmpty,

        [InfoAttribute(400, "Seniority name must be between 2 and 200 characters")]
        SeniorityNameLengthInvalid,

        [InfoAttribute(400, "Invalid organization identifier")]
        InvalidOrganizationId,

        [InfoAttribute(400, "Invalid sort order")]
        InvalidSortOrder,

        [InfoAttribute(400, "Area level name must be between 2 and 200 characters")]
        AreaLevelNameLengthInvalid,

        [InfoAttribute(400, "Skill name must be between 2 and 150 characters")]
        SkillNameLengthInvalid,

        [InfoAttribute(400, "Invalid skill category identifier")]
        InvalidSkillCategoryId,

        [InfoAttribute(400, "Skill category name must be between 2 and 100 characters")]
        SkillCategoryNameLengthInvalid,

        [InfoAttribute(400, "Invalid profile identifier")]
        InvalidProfileId,

        [InfoAttribute(400, "Invalid skill identifier")]
        InvalidSkillId,

        [InfoAttribute(400, "Invalid skill level identifier")]
        InvalidSkillLevelId,

        [InfoAttribute(400, "Skill weight must be 'obligatorio' or 'deseable'")]
        InvalidSkillWeight,

        [InfoAttribute(400, "Vacancy status name must be between 2 and 100 characters")]
        VacancyStatusNameLengthInvalid,

        [InfoAttribute(400, "Vacancy status description is invalid")]
        VacancyStatusDescriptionInvalid,

        [InfoAttribute(400, "Vacancy title must be between 3 and 200 characters")]
        VacancyTitleLengthInvalid,

        [InfoAttribute(400, "Invalid client name")]
        InvalidClientName,

        [InfoAttribute(400, "Invalid project name")]
        InvalidProjectName,

        [InfoAttribute(400, "Invalid vacancy location")]
        InvalidVacancyLocation,

        [InfoAttribute(400, "Invalid criticality level identifier")]
        InvalidCriticalityLevelId,

        [InfoAttribute(400, "Invalid vacancy status identifier")]
        InvalidVacancyStatusId,

        [InfoAttribute(400, "Invalid position count")]
        InvalidPositionCount,

        [InfoAttribute(400, "Invalid salary range")]
        InvalidSalaryRange,

        [InfoAttribute(400, "Invalid request date")]
        InvalidRequestDate,

        [InfoAttribute(400, "Criticality level name must be between 2 and 200 characters")]
        CriticalityLevelNameLengthInvalid,

        [InfoAttribute(400, "Skill level name must be between 2 and 200 characters")]
        SkillLevelNameLengthInvalid,

        [InfoAttribute(400, "Profile  name must be between 2 and 200 characters")]
        ProfileNameLengthInvalid,

        #endregion

        #region 404 Not Found

        [InfoAttribute(404, "User Not Found")]
        UserNotFound,

        [InfoAttribute(404, "Role Not Found")]
        RoleNotFound,

        [InfoAttribute(404, "Permission Not Found")]
        PermissionNotFound,

        [InfoAttribute(404, "Seniority Level Not Found")]
        SeniorityLevelNotFound,

        [InfoAttribute(404, "Area Level Not Found")]
        AreaLevelNotFound,

        [InfoAttribute(404, "Criticality Level Not Found")]
        CriticalityLevelNotFound,

        [InfoAttribute(404, "Skill Level Not Found")]
        SkillLevelNotFound,

        [InfoAttribute(404, "Skill Category Not Found")]
        SkillCategoryNotFound,

        [InfoAttribute(404, "Skill Not Found")]
        SkillNotFound,
        ProfileSkillNotFound,

        [InfoAttribute(404, "Vacancy Not Found")]
        VacancyNotFound,

        [InfoAttribute(404, "Vacancy Status Not Found")]
        VacancyStatusNotFound,

        [InfoAttribute(404, "Profile Not Found")]
        ProfileNotFound,


        #endregion

        #region 500 - Internal Server Error

        [InfoAttribute(500, "Internal Server Error")]
        InternalServerError,


        #endregion
    }
}
