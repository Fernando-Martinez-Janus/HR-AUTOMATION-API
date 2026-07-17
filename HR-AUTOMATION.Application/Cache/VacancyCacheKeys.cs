using HR_AUTOMATION.Application.InputModels;
using Shared.Kernel.Utils.Helpers;

namespace HR_AUTOMATION.Application.Cache
{
    public static class VacancyCacheKeys
    {
        public static string ById(int id) => $"{CacheKeys.VacancyKey}:{CacheKeys.IdKey}:{id}";

        public static string Version(int? organizationId = null) =>
            organizationId.HasValue
            ? $"{CacheKeys.VacancyKey}:{CacheKeys.OrganizationKey}:{organizationId.Value}:{CacheKeys.VersionKey}"
            : $"{CacheKeys.VacancyKey}:{CacheKeys.OrganizationKey}:{CacheKeys.AllOrganizationsKey}:{CacheKeys.VersionKey}";

        public static string Search(VacancySearchInputModel model, string version)
        {
            string scope = model?.OrganizationId?.ToString() ?? CacheKeys.AllOrganizationsKey;
            string searchHashed = string.IsNullOrEmpty(model?.SearchTerm) ? CacheKeys.SearchEmptyKey : CacheKeyHelper.ComputeHash(model.SearchTerm);

            return string.Concat(
                $"{CacheKeys.VacancyKey}:",
                $"{CacheKeys.OrganizationKey}:{scope}:",
                $"{CacheKeys.VersionKey}:{version}:",
                $"{CacheKeys.SearchKey}:{searchHashed}:",
                $"{CacheKeys.PageNumberKey}:{model?.PageNumber}:",
                $"{CacheKeys.PageSizeKey}:{model?.PageSize}"
            );
        }
    }
}
