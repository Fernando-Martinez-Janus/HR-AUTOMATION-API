namespace Shared.Kernel.IRepositories
{
    /// <summary>
    /// Interface that defines methods for executing stored procedures and retrieving results from the database.
    /// </summary>
    public interface ISharedRepository
    {
        /// <summary>
        /// Executes a stored procedure asynchronously and retrieves a collection of results mapped to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the result set should be mapped.</typeparam>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an enumerable collection of type T.</returns>
        Task<IEnumerable<T>> QueryAsync<T>(string spName, List<KeyValuePair<string, object?>>? parameters = null) where T : new();

        /// <summary>
        /// Executes a stored procedure asynchronously and retrieves a scalar value (e.g., single value such as sum, count, etc.).
        /// </summary>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the scalar value returned from the stored procedure, or null if no value is returned.</returns>
        Task<object?> QueryScalarAsync(string spName, List<KeyValuePair<string, object?>>? parameters = null);

        /// <summary>
        /// Executes a stored procedure asynchronously and returns a boolean indicating success or failure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        Task ExecuteAsync(string spName, List<KeyValuePair<string, object?>>? parameters = null);
    }
}
