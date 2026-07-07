using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Kernel.Appsettings;
using Shared.Kernel.IRepositories;
using Shared.Kernel.Utils.Helpers;
using System.Data;

namespace Shared.Kernel.Repositories
{
    /// <summary>
    /// This class provides methods for executing stored procedures and retrieving results from the database.
    /// </summary>
    /// <param name="logger">Instance of Serilog Logger.</param>
    /// <param name="connectionConfigurations">Instance of IOption to obtain appsettings' connection configurations.</param>
    public class SharedRepository(ILogger<SharedRepository> logger, IOptions<ConnectionConfigurations> connectionConfigurations) : ISharedRepository
    {
        /// <summary>
        /// Used for logging error and information messages.
        /// </summary>
        private readonly ILogger<SharedRepository> _logger = logger;

        /// <summary>
        /// Obtains connection's information.
        /// </summary>
        private readonly ConnectionConfigurations _connectionConfigurations = connectionConfigurations.Value;

        /// <summary>
        /// Executes a stored procedure asynchronously and retrieves a collection of results mapped to the specified type.
        /// </summary>
        /// <typeparam name="T">The type to which the result set should be mapped.</typeparam>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is an enumerable collection of type T.</returns>
        public async Task<IEnumerable<T>> QueryAsync<T>(string spName, List<KeyValuePair<string, object?>>? parameters = null) where T : new()
        {

            try
            {
                using SqlConnection connection = new(_connectionConfigurations.SqlServerConnection.ConnectionString);
                using SqlCommand command = new(spName, connection);

                await connection.OpenAsync();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = _connectionConfigurations.SqlServerConnection.Timeout;

                parameters?.ForEach(parameter =>
                {
                    SqlParameter sqlParameter = new(parameter.Key, parameter.Value ?? DBNull.Value);

                    command.Parameters.Add(sqlParameter);
                });

                using SqlDataReader _reader = await command.ExecuteReaderAsync();


                List<T> items = [];
                RowMapper<T> mapper = DbReaderMapper.Bind<T>(_reader);

                while (await _reader.ReadAsync())
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
                _logger.LogError("{message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure asynchronously and retrieves a scalar value (e.g., single value such as sum, count, etc.).
        /// </summary>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is the scalar value returned from the stored procedure, or null if no value is returned.</returns>
        public async Task<object?> QueryScalarAsync(string spName, List<KeyValuePair<string, object?>>? parameters = null)
        {
            try
            {
                using SqlConnection connection = new(_connectionConfigurations.SqlServerConnection.ConnectionString);
                using SqlCommand command = new(spName, connection);

                await connection.OpenAsync();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = _connectionConfigurations.SqlServerConnection.Timeout;

                parameters?.ForEach(parameter =>
                {
                    SqlParameter sqlParameter = new(parameter.Key, parameter.Value ?? DBNull.Value);

                    command.Parameters.Add(sqlParameter);
                });

                object? result = await command.ExecuteScalarAsync();

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
                _logger.LogError("{message}", ex.Message);
                throw;
            }
        }

        /// <summary>
        /// Executes a stored procedure asynchronously and returns a boolean indicating success or failure.
        /// </summary>
        /// <param name="spName">The name of the stored procedure to be executed.</param>
        /// <param name="parameters">Optional list of parameters to pass to the stored procedure.</param>
        /// <returns>A task that represents the asynchronous operation. The task result is true if the stored procedure executed successfully, otherwise false.</returns>
        public async Task ExecuteAsync(string spName, List<KeyValuePair<string, object?>>? parameters = null)
        {
            try
            {
                using SqlConnection connection = new(_connectionConfigurations.SqlServerConnection.ConnectionString);
                using SqlCommand command = new(spName, connection);

                await connection.OpenAsync();
                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = _connectionConfigurations.SqlServerConnection.Timeout;

                parameters?.ForEach(parameter =>
                {
                    SqlParameter sqlParameter = new(parameter.Key, parameter.Value ?? DBNull.Value);

                    command.Parameters.Add(sqlParameter);
                });

                await command.ExecuteNonQueryAsync();
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
                _logger.LogError("{message}", ex.Message);
                throw;
            }
        }
    }
}
