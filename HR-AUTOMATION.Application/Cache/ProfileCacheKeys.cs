namespace HR_AUTOMATION.Application.Cache;

public static class ProfileCacheKeys
{
    public static string ById(int id) =>
        $"{CacheKeys.ProfileKey}:{CacheKeys.IdKey}:{id}";

    public static string All(int organizationId) =>
        $"{CacheKeys.ProfileKey}:{CacheKeys.OrganizationKey}:{organizationId}:{CacheKeys.AllKey}";

    public static string All(int organizationId, string version) =>
        $"{CacheKeys.ProfileKey}:{CacheKeys.OrganizationKey}:{organizationId}:{CacheKeys.AllKey}:{version}";

    public static string Version(int? organizationId = null) =>
        organizationId.HasValue
        ? $"{CacheKeys.ProfileKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
        : $"{CacheKeys.ProfileKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";
}