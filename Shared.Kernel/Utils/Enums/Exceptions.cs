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


        #endregion

        #region 404 Not Found
        [EnumInfo(404, "Skill Category Not Found")]
        SkillCategoryNotFound,
        [EnumInfo(404, "Skill Not Found")]
        SkillNotFound,
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