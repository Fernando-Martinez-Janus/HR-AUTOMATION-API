using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    public static class ScolarityLevelCacheKeys
    {
        public static string ById(int id) => $"{CacheKeys.ScolarityLevelKey}:{CacheKeys.IdKey}:{id}";

        public static string Version(int? organizationId = null) =>
            organizationId.HasValue
            ? $"{CacheKeys.ScolarityLevelKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
            : $"{CacheKeys.ScolarityLevelKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

        public static string Search(ScolarityLevelSearchInputModel model, string version)
        {
            string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.ScolarityLevelKey}:",
                $"{CacheKeys.OrganizationKey}:{scope}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}"
            );
        }
    }
}
