namespace Shared.Kernel.Utils.Enums
{
    public enum Exceptions
    {
        #region 400 - Bad Request

        [EnumInfo(400, "Organization is required")]
        OrganizationRequired,
        [EnumInfo(400, "Name is required")]
        NameRequired,
        [EnumInfo(400, "Skill Category name must not exceed 100 characters")]
        SkillCategoryNameLengthInvalid,
        [EnumInfo(400, "Skill Category icon name must not exceed 50 characters")]
        SkillCategoryIconNameLengthInvalid,
        [EnumInfo(400, "Skill name must not exceed 150 characters")]
        SkillNameLengthInvalid,
        [EnumInfo(400, "Skill Level name must not exceed 100 characters")]
        SkillLevelNameLengthInvalid,
        [EnumInfo(400, "Skill Level description must not exceed 300 characters")]
        SkillLevelDescriptionLengthInvalid,
        [EnumInfo(400, "Area Level name must not exceed 100 characters")]
        AreaLevelNameLengthInvalid,
        [EnumInfo(400, "Area Level description must not exceed 300 characters")]
        AreaLevelDescriptionLengthInvalid,
        [EnumInfo(400, "Criticality Level name must not exceed 100 characters")]
        CriticalityLevelNameLengthInvalid,
        [EnumInfo(400, "Criticality Level description must not exceed 300 characters")]
        CriticalityLevelDescriptionLengthInvalid,
        [EnumInfo(400, "Rejection Reason description must not exceed 300 characters")]
        RejectionReasonDescriptionLengthInvalid,
        [EnumInfo(400, "Currency code must not exceed 3 characters")]
        CurrencyCodeLengthInvalid,
        [EnumInfo(400, "Currency name must not exceed 100 characters")]
        CurrencyNameLengthInvalid,
        [EnumInfo(400, "Currency symbol must not exceed 5 characters")]
        CurrencySymbolLengthInvalid,
        [EnumInfo(400, "Payment Period name must not exceed 100 characters")]
        PaymentPeriodNameLengthInvalid,
        [EnumInfo(400, "Employment Type name must not exceed 100 characters")]
        EmploymentTypeNameLengthInvalid,
        [EnumInfo(400, "Work Modality name must not exceed 100 characters")]
        WorkModalityNameLengthInvalid,
        [EnumInfo(400, "Seniority Level name must not exceed 100 characters")]
        SeniorityLevelNameLengthInvalid,
        [EnumInfo(400, "Question Category name must not exceed 100 characters")]
        QuestionCategoryNameLengthInvalid,
        [EnumInfo(400, "Question Category description must not exceed 300 characters")]
        QuestionCategoryDescriptionLengthInvalid,
        [EnumInfo(400, "Organization name must not exceed 150 characters")]
        OrganizationNameLengthInvalid,
        [EnumInfo(400, "Organization slug must not exceed 50 characters")]
        OrganizationSlugLengthInvalid,
        [EnumInfo(400, "Vacancy title is required")]
        VacancyTitleRequired,
        [EnumInfo(400, "Vacancy title must not exceed 200 characters")]
        VacancyTitleLengthInvalid,
        [EnumInfo(400, "Position count must be greater than 0")]
        VacancyPositionCountInvalid,

        #endregion

        #region 404 Not Found
        [EnumInfo(404, "Skill Category Not Found")]
        SkillCategoryNotFound,
        [EnumInfo(404, "Skill Not Found")]
        SkillNotFound,
        [EnumInfo(404, "Skill Level Not Found")]
        SkillLevelNotFound,
        [EnumInfo(404, "Area Level Not Found")]
        AreaLevelNotFound,
        [EnumInfo(404, "Criticality Level Not Found")]
        CriticalityLevelNotFound,
        [EnumInfo(404, "Rejection Reason Not Found")]
        RejectionReasonNotFound,
        [EnumInfo(404, "Currency Not Found")]
        CurrencyNotFound,
        [EnumInfo(404, "Payment Period Not Found")]
        PaymentPeriodNotFound,
        [EnumInfo(404, "Employment Type Not Found")]
        EmploymentTypeNotFound,
        [EnumInfo(404, "Work Modality Not Found")]
        WorkModalityNotFound,
        [EnumInfo(404, "Seniority Level Not Found")]
        SeniorityLevelNotFound,
        [EnumInfo(404, "Question Category Not Found")]
        QuestionCategoryNotFound,
        [EnumInfo(404, "Organization Not Found")]
        OrganizationNotFound,
        [EnumInfo(404, "Vacancy Not Found")]
        VacancyNotFound,
        #endregion

        #region 429 - Too Many Requests
        [EnumInfo(429, "Too many requests. Please try again later.")]
        TooManyRequests,
        #endregion

        #region 500 - Internal Server Error
        [EnumInfo(500, "Internal Server Error")]
        InternalServerError,
        #endregion
    }
}