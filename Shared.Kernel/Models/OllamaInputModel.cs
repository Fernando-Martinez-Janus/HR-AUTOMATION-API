using Newtonsoft.Json;

namespace Shared.Kernel.Models
{
    /// <summary>
    /// Represents the request body sent to Ollama's /api/generate endpoint.
    /// </summary>
    public class OllamaInputModel
    {
        /// <summary>The model name (e.g., "llama3.2:3b").</summary>
        [JsonProperty("model")]
        public string Model { get; set; } = string.Empty;

        /// <summary>The input prompt for the model.</summary>
        [JsonProperty("prompt")]
        public string Prompt { get; set; } = string.Empty;

        /// <summary>Whether to stream the response. Set to false for a single response.</summary>
        [JsonProperty("stream")]
        public bool Stream { get; set; }
    }
}
