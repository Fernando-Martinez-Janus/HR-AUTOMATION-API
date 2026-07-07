namespace Shared.Kernel.Appsettings
{
    /// <summary>
    /// Connection configurations for databases.
    /// </summary>
    public class ConnectionConfigurations
    {
        /// <summary>
        /// Connection configuration for SQL Server.
        /// </summary>
        public ConnectionConfiguration SqlServerConnection { get; set; } = new();
    }

    /// <summary>
    /// Configuration for certain database.
    /// </summary>
    public class ConnectionConfiguration
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        public string ConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Time in seconds to wait for the commando to execute.
        /// </summary>
        public int Timeout { get; set; }
    }
}
