using Shared.Kernel.Utils.Constants;

namespace Shared.Kernel.InputModels
{
    /// <summary>
    /// Represents the parameters used to request paginated data.
    /// </summary>
    public class PaginationRequest
    {
        /// <summary>
        /// Gets or sets the search term used to filter the results.
        /// </summary>
        public string? SearchTerm { get; set; }

        /// <summary>
        /// Gets or sets the page number to retrieve.
        /// Defaults to <see cref="AppConstants.DefaultPageNumber"/>.
        /// </summary>
        public int PageNumber { get; set; } = AppConstants.DefaultPageNumber;

        /// <summary>
        /// Gets or sets the maximum number of items to return per page.
        /// Defaults to <see cref="AppConstants.DefaultPageSize"/>.
        /// </summary>
        public int PageSize { get; set; } = AppConstants.DefaultPageSize;

        // <summary>
        /// Cleans and normalizes input values.
        /// </summary>
        public virtual void Normalize()
        {
            if (PageNumber < 1)
            {
                PageNumber = AppConstants.DefaultPageNumber;
            }

            if (PageSize < 1)
            {
                PageSize = AppConstants.DefaultPageSize;
            }

            SearchTerm = string.IsNullOrWhiteSpace(SearchTerm) ? null : SearchTerm.Trim().ToLower();
        }
    }
}