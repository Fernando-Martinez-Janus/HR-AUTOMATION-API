namespace Shared.Kernel.Appsettings
{
    /// <summary>
    /// Configuration settings for connecting to an Ollama server.
    /// </summary>
    public class OllamaConfigurations
    {
        /// <summary>Base URL of the Ollama server (e.g., "http://192.168.1.100:11434").</summary>
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>Endpoint path for text generation (e.g., "/api/generate").</summary>
        public string GenerateEndpoint { get; set; } = "/api/generate";

        /// <summary>Request timeout in seconds.</summary>
        public int Timeout { get; set; } = 100;
    }
}
