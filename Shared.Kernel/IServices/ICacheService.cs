namespace Shared.Kernel.IServices
{
    /// <summary>
    /// Defines a contract for interacting with a cache provider.
    /// </summary>
    /// <remarks>
    /// Provides asynchronous methods for storing, retrieving, and removing cached values regardless of the underlying cache implementation.
    /// </remarks>
    public interface ICacheService
    {
        /// <summary>
        /// Retrieves a value from the cache by its key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="key">The unique cache key.</param>
        /// <returns>The cached value if it exists; otherwise, <c>null</c>.</returns>
        Task<T?> GetAsync<T>(string key);

        /// <summary>
        /// Stores a value in the cache.
        /// </summary>
        /// <typeparam name="T">The type of the value to cache.</typeparam>
        /// <param name="key">The unique cache key.</param>
        /// <param name="value">The value to store.</param>
        /// <param name="expiration">
        /// The optional expiration time for the cached value. 
        /// If <c>null</c>, the default expiration policy is used.
        /// </param>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null);

        /// <summary>
        /// Removes a value from the cache.
        /// </summary>
        /// <param name="key">The unique cache key.</param>
        Task DeleteAsync(string key);
    }
}
