namespace HR_AUTOMATION.Application.Cache;

public static class ProfileCacheKeys
{
    public static string ById(int id) => $"{CacheKeys.ProfileKey}:{CacheKeys.IdKey}:{id}";

    public static string Version(int? organizationId = null) =>
        organizationId.HasValue
        ? $"{CacheKeys.ProfileKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
        : $"{CacheKeys.ProfileKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";
}
