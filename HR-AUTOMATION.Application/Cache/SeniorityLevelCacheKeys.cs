using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    /// <summary>
    /// Provides methods to generate cache keys for seniority level operations.
    /// </summary>
    public static class SeniorityLevelCacheKeys
    {
        /// <summary>
        /// Generates a cache key for a specific seniority level identifier.
        /// </summary>
        public static string ById(int id) => $"{CacheKeys.SeniorityLevelKey}:{CacheKeys.IdKey}:{id}";

        /// <summary>
        /// Generates a cache key for the seniority level version by organization scope.
        /// </summary>
        public static string Version(int? organizationId = null) =>
            organizationId.HasValue
            ? $"{CacheKeys.SeniorityLevelKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
            : $"{CacheKeys.SeniorityLevelKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

        /// <summary>
        /// Generates a cache key for seniority level search results based on the provided filters.
        /// </summary>
        public static string Search(SeniorityLevelSearchInputModel model, string version)
        {
            string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.SeniorityLevelKey}:",
                $"{CacheKeys.OrganizationKey}:{scope}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}"
            );
        }
    }
}
