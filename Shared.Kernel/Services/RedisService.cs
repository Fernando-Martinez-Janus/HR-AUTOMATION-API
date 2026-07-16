using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.Kernel.IServices;
using StackExchange.Redis;

namespace Shared.Kernel.Services
{
    /// <summary>
    /// Provides a Redis-based implementation of <see cref="ICacheService"/>.
    /// </summary>
    /// <param name="logger">The logger used to record cache operations and errors.</param>
    /// <param name="connection">The shared Redis connection multiplexer.</param>
    public class RedisService(ILogger<RedisService> logger, IConnectionMultiplexer connection) : ICacheService
    {
        /// <summary>
        /// Logger used for diagnostic and error logging.
        /// </summary>
        private readonly ILogger<RedisService> _logger = logger;

        /// <summary>
        /// Represents the Redis database used for cache operations.
        /// </summary>
        private readonly IDatabase _database = connection.GetDatabase();

        /// <summary>
        /// Retrieves a value from the cache by its key.
        /// </summary>
        /// <typeparam name="T">The type of the cached value.</typeparam>
        /// <param name="key">The unique cache key.</param>
        /// <returns>The cached value if it exists; otherwise, <c>null</c>.</returns>
        public async Task<T?> GetAsync<T>(string key)
        {
            try
            {
                RedisValue value = await _database.StringGetAsync(key);

                if (value.IsNullOrEmpty)
                {
                    return default;
                }

                return JsonConvert.DeserializeObject<T>(value!);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(GetAsync));
                return default;
            }
        }

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
        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(value);

                if (expiration.HasValue)
                {
                    await _database.StringSetAsync(key, json, expiration.Value);

                    return;
                }

                await _database.StringSetAsync(key, json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(SetAsync));
            }
        }

        /// <summary>
        /// Removes a value from the cache.
        /// </summary>
        /// <param name="key">The unique cache key.</param>
        public async Task DeleteAsync(string key)
        {
            try
            {
                await _database.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(DeleteAsync));
            }
        }
    }
}