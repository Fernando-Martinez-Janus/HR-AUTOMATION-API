using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Shared.Kernel.IRepositories;
using Shared.Kernel.Utils.Constants;
using Shared.Kernel.Utils.Helpers;
using System.Data;

namespace Shared.Kernel.Repositories
{
    /// <summary>
    /// This class provides methods for executing stored procedures and retrieving results from the database.
    /// </summary>
    /// <param name="logger">Instance of Serilog Logger.</param>
    /// <param name="configuration">Instance of IConfiguration to obtain appsettings' connection configurations.</param>
    public class SqlServerRepository(ILogger<SqlServerRepository> logger, IConfiguration configuration) : ISharedRepository
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<SqlServerRepository> _logger = logger;

        /// <summary>
        /// Obtains connection's information.
        /// </summary>
        private readonly IConfiguration _configuration = configuration;

        /// <summary>
        /// Executes a stored procedure asynchronously and retrieves a collection of results mapped to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the result set should be mapped.</typeparam>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the database operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an enumerable collection of type T.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string spName, List<KeyValuePair<string, object?>>? parameters = null, CancellationToken cancellationToken = default) where T : new()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>(AppConstants.SqlServerConnectionStringKey)!;
                int timeout = _configuration.GetValue<int>(AppConstants.SqlServerTimeoutKey)!;

                using SqlConnection connection = new(connectionString);
                using SqlCommand command = new(spName, connection);

                await connection.OpenAsync(cancellationToken);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = timeout;

                parameters?.ForEach(parameter =>
                {
                    SqlParameter sqlParameter = new(parameter.Key, parameter.Value ?? DBNull.Value);

                    command.Parameters.Add(sqlParameter);
                });

                using SqlDataReader _reader = await command.ExecuteReaderAsync(cancellationToken);

                List<T> items = [];
                RowMapper<T> mapper = DbReaderMapper.Bind<T>(_reader);

                while (await _reader.ReadAsync(cancellationToken))
                {
                    items.Add(mapper.Map(_reader));
                }

                return items;
            }
            catch (SqlException sqlEx)
            {
                // Log the SQL error message in the log files and re-throw the exception.
                _logger.LogError("SQL Error executing {StoredProcedure}: {Message}", spName, sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                // Log the error message in the log files and re-throw the exception.
                _logger.LogError(ex, nameof(QueryAsync));
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure asynchronously and retrieves a single result mapped to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the result set should be mapped.</typeparam>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the database operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an instance of type T, or null if no record is found.</returns>
        public async Task<T?> QuerySingleAsync<T>(string spName, List<KeyValuePair<string, object?>>? parameters = null, CancellationToken cancellationToken = default) where T : new()
        {
            try
            {
                string connectionString = _configuration.GetValue<string>(AppConstants.SqlServerConnectionStringKey)!;
                int timeout = _configuration.GetValue<int>(AppConstants.SqlServerTimeoutKey)!;

                using SqlConnection connection = new(connectionString);
                using SqlCommand command = new(spName, connection);

                await connection.OpenAsync(cancellationToken);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = timeout;

                parameters?.ForEach(parameter =>
                {
                    SqlParameter sqlParameter = new(parameter.Key, parameter.Value ?? DBNull.Value);
                    command.Parameters.Add(sqlParameter);
                });

                using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

                if (!await reader.ReadAsync(cancellationToken))
                {
                    return default;
                }

                RowMapper<T> mapper = DbReaderMapper.Bind<T>(reader);

                return mapper.Map(reader);
            }
            catch (SqlException sqlEx)
            {
                // Log the SQL error message in the log files and re-throw the exception.
                _logger.LogError("SQL Error executing {StoredProcedure}: {Message}", spName, sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                // Log the error message in the log files and re-throw the exception.
                _logger.LogError(ex, nameof(QueryScalarAsync));
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure asynchronously and retrieves a scalar value (e.g., single value such as sum, count, etc.).
        /// </summary>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the database operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the scalar value returned from the stored procedure, or null if no value is returned.</returns>
        public async Task<object?> QueryScalarAsync(string spName, List<KeyValuePair<string, object?>>? parameters = null, CancellationToken cancellationToken = default)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>(AppConstants.SqlServerConnectionStringKey)!;
                int timeout = _configuration.GetValue<int>(AppConstants.SqlServerTimeoutKey)!;

                using SqlConnection connection = new(connectionString);
                using SqlCommand command = new(spName, connection);

                await connection.OpenAsync(cancellationToken);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = timeout;

                parameters?.ForEach(parameter =>
                {
                    SqlParameter sqlParameter = new(parameter.Key, parameter.Value ?? DBNull.Value);

                    command.Parameters.Add(sqlParameter);
                });

                object? result = await command.ExecuteScalarAsync(cancellationToken);

                if (result == DBNull.Value)
                {
                    return null;
                }

                return result;
            }
            catch (SqlException sqlEx)
            {
                // Log the SQL error message in the log files and re-throw the exception.
                _logger.LogError("SQL Error executing {StoredProcedure}: {Message}", spName, sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                // Log the error message in the log files and re-throw the exception.
                _logger.LogError(ex, nameof(QueryScalarAsync));
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure asynchronously and returns a boolean indicating success or failure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the database operation.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the stored procedure executed successfully, otherwise false.</returns>
        public async Task ExecuteAsync(string spName, List<KeyValuePair<string, object?>>? parameters = null, CancellationToken cancellationToken = default)
        {
            try
            {
                string connectionString = _configuration.GetValue<string>(AppConstants.SqlServerConnectionStringKey)!;
                int timeout = _configuration.GetValue<int>(AppConstants.SqlServerTimeoutKey)!;

                using SqlConnection connection = new(connectionString);
                using SqlCommand command = new(spName, connection);

                await connection.OpenAsync(cancellationToken);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = timeout;

                parameters?.ForEach(parameter =>
                {
                    SqlParameter sqlParameter = new(parameter.Key, parameter.Value ?? DBNull.Value);

                    command.Parameters.Add(sqlParameter);
                });

                await command.ExecuteNonQueryAsync(cancellationToken);
            }
            catch (SqlException sqlEx)
            {
                // Log the SQL error message in the log files and re-throw the exception.
                _logger.LogError("SQL Error executing {StoredProcedure}: {Message}", spName, sqlEx.Message);
                throw;
            }
            catch (Exception ex)
            {
                // Log the error message in the log files and re-throw the exception.
                _logger.LogError(ex, nameof(ExecuteAsync));
                throw;
            }
        }
    }
}