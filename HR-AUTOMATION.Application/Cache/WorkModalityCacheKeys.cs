using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    /// <summary>
    /// Provides methods to generate cache keys for work modality operations.
    /// </summary>
    public static class WorkModalityCacheKeys
    {
        /// <summary>
        /// Generates a cache key for a specific work modality identifier.
        /// </summary>
        public static string ById(int id) => $"{CacheKeys.WorkModalityKey}:{CacheKeys.IdKey}:{id}";

        /// <summary>
        /// Generates a cache key for the work modality version by organization scope.
        /// </summary>
        public static string Version(int? organizationId = null) =>
            organizationId.HasValue
            ? $"{CacheKeys.WorkModalityKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
            : $"{CacheKeys.WorkModalityKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

        /// <summary>
        /// Generates a cache key for work modality search results based on the provided filters.
        /// </summary>
        public static string Search(WorkModalitySearchInputModel model, string version)
        {
            string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.WorkModalityKey}:",
                $"{CacheKeys.OrganizationKey}:{scope}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}"
            );
        }
    }
}
