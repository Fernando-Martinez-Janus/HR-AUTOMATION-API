using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache;

/// <summary>
/// Provides methods to generate cache keys for profile operations.
/// </summary>
public static class ProfileCacheKeys
{
    /// <summary>
    /// Generates a cache key for a specific profile identifier.
    /// </summary>
    public static string ById(int id) =>
        $"{CacheKeys.ProfileKey}:{CacheKeys.IdKey}:{id}";

    /// <summary>
    /// Generates a cache key for the profile by organization scope.
    /// </summary>
    public static string Version(int? organizationId = null) =>
        organizationId.HasValue
        ? $"{CacheKeys.ProfileKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
        : $"{CacheKeys.ProfileKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

    /// <summary>
    /// Generates a cache key for profile search results based on the provided filters.
    /// </summary>
    public static string Search(ProfileSearchInputModel model, string version)
    {
        string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
        string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

        return string.Concat(
            $"{CacheKeys.SkillKey}:",
            $"{CacheKeys.OrganizationKey}:{scope}:",
            $"{CacheKeys.VersionKey}:{version}:",
            $"{CacheKeys.SearchKey}:{searchHashed}:",
            $"{CacheKeys.AreaLevelFilter}:{model?.AreaLevelId}:",
            $"{CacheKeys.SeniorityLevelFilter}:{model?.SeniorityLevelId}:",
            $"{CacheKeys.PageNumberKey}:{model?.PageNumber}:",
            $"{CacheKeys.PageSizeKey}:{model?.PageSize}"
        );
    }
}