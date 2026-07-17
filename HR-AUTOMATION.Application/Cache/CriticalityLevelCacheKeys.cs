using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    /// <summary>
    /// Provides methods to generate cache keys for criticality level operations.
    /// </summary>
    public static class CriticalityLevelCacheKeys
    {
        /// <summary>
        /// Generates a cache key for a specific criticality level identifier.
        /// </summary>
        public static string ById(int id) => $"{CacheKeys.CriticalityLevelKey}:{CacheKeys.IdKey}:{id}";

        /// <summary>
        /// Generates a cache key for the criticality level version by organization scope.
        /// </summary>
        public static string Version(int? organizationId = null) =>
            organizationId.HasValue
            ? $"{CacheKeys.CriticalityLevelKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
            : $"{CacheKeys.CriticalityLevelKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

        /// <summary>
        /// Generates a cache key for criticality level search results based on the provided filters.
        /// </summary>
        public static string Search(CriticalityLevelSearchInputModel model, string version)
        {
            string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.CriticalityLevelKey}:",
                $"{CacheKeys.OrganizationKey}:{scope}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}"
            );
        }
    }
}
