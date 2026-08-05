namespace HR_AUTOMATION.Application.Cache
{
    public class SearchResultsCacheKeys
    {
        public static string Version(int? organizationId = null) =>
          organizationId.HasValue
              ? $"{CacheKeys.SearchRequestKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
              : $"{CacheKeys.SearchRequestKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";
    }
}
