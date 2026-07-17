using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    /// <summary>
    /// Provides methods to generate cache keys for payment period operations.
    /// </summary>
    public static class PaymentPeriodCacheKeys
    {
        /// <summary>
        /// Generates a cache key for a specific payment period identifier.
        /// </summary>
        public static string ById(int id) => $"{CacheKeys.PaymentPeriodKey}:{CacheKeys.IdKey}:{id}";

        /// <summary>
        /// Generates a cache key for the payment period version by organization scope.
        /// </summary>
        public static string Version(int? organizationId = null) =>
            organizationId.HasValue
            ? $"{CacheKeys.PaymentPeriodKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
            : $"{CacheKeys.PaymentPeriodKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

        /// <summary>
        /// Generates a cache key for payment period search results based on the provided filters.
        /// </summary>
        public static string Search(PaymentPeriodSearchInputModel model, string version)
        {
            string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.PaymentPeriodKey}:",
                $"{CacheKeys.OrganizationKey}:{scope}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}"
            );
        }
    }
}
