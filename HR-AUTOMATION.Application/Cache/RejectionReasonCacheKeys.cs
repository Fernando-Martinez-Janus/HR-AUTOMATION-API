using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    /// <summary>
    /// Provides methods to generate cache keys for rejection reason operations.
    /// </summary>
    public static class RejectionReasonCacheKeys
    {
        /// <summary>
        /// Generates a cache key for a specific rejection reason identifier.
        /// </summary>
        public static string ById(int id) => $"{CacheKeys.RejectionReasonKey}:{CacheKeys.IdKey}:{id}";

        /// <summary>
        /// Generates a cache key for the rejection reason version by organization scope.
        /// </summary>
        public static string Version(int? organizationId = null) =>
            organizationId.HasValue
            ? $"{CacheKeys.RejectionReasonKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
            : $"{CacheKeys.RejectionReasonKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

        /// <summary>
        /// Generates a cache key for rejection reason search results based on the provided filters.
        /// </summary>
        public static string Search(RejectionReasonSearchInputModel model, string version)
        {
            string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.RejectionReasonKey}:",
                $"{CacheKeys.OrganizationKey}:{scope}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}:",
                $"{CacheKeys.PageNumberKey}:{model?.PageNumber}:",
                $"{CacheKeys.PageSizeKey}:{model?.PageSize}"
            );
        }
    }
}
