using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    /// <summary>
    /// Provides methods to generate cache keys for question category operations.
    /// </summary>
    public static class QuestionCategoryCacheKeys
    {
        /// <summary>
        /// Generates a cache key for a specific question category identifier.
        /// </summary>
        public static string ById(int id) => $"{CacheKeys.QuestionCategoryKey}:{CacheKeys.IdKey}:{id}";

        /// <summary>
        /// Generates a cache key for the question category version by organization scope.
        /// </summary>
        public static string Version(int? organizationId = null) =>
            organizationId.HasValue
            ? $"{CacheKeys.QuestionCategoryKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
            : $"{CacheKeys.QuestionCategoryKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

        /// <summary>
        /// Generates a cache key for question category search results based on the provided filters.
        /// </summary>
        public static string Search(QuestionCategorySearchInputModel model, string version)
        {
            string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.QuestionCategoryKey}:",
                $"{CacheKeys.OrganizationKey}:{scope}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}:",
                $"{CacheKeys.PageNumberKey}:{model?.PageNumber}:",
                $"{CacheKeys.PageSizeKey}:{model?.PageSize}"
            );
        }
    }
}
