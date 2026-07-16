namespace Shared.Kernel.ViewModels
{
    /// <summary>
    /// Represents a paginated response containing the total number of available
    /// records and the data for the requested page.
    /// </summary>
    /// <typeparam name="T">The type of the items included in the response.</typeparam>
    /// <param name="totalCount">
    /// The total number of records available before pagination is applied.
    /// </param>
    /// <param name="data">
    /// The collection of items for the requested page.
    /// </param>
    public class PaginationResponse<T>(int totalCount, IEnumerable<T> data)
    {
        /// <summary>
        /// Gets or sets the total number of available records.
        /// </summary>
        public int TotalCount { get; set; } = totalCount;

        /// <summary>
        /// Gets or sets the collection of items for the current page.
        /// </summary>
        public IEnumerable<T> Data { get; set; } = data;
    }
}