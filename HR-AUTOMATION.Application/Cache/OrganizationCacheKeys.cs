using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    /// <summary>
    /// Provides methods to generate cache keys for organization operations.
    /// </summary>
    public static class OrganizationCacheKeys
    {
        /// <summary>
        /// Generates a cache key for a specific organization identifier.
        /// </summary>
        public static string ById(int id) => $"{CacheKeys.OrganizationEntityKey}:{CacheKeys.IdKey}:{id}";

        /// <summary>
        /// Generates a cache key for the organization list version.
        /// </summary>
        public static string Version() => $"{CacheKeys.OrganizationEntityKey}:{CacheKeys.VersionKey}";

        /// <summary>
        /// Generates a cache key for organization search results based on the provided filters.
        /// </summary>
        public static string Search(OrganizationSearchInputModel model, string version)
        {
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.OrganizationEntityKey}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}:",
                $"{CacheKeys.PageNumberKey}:{model?.PageNumber}:",
                $"{CacheKeys.PageSizeKey}:{model?.PageSize}"
            );
        }
    }
}
