using Newtonsoft.Json;

namespace Shared.Kernel.Models
{
    /// <summary>
    /// Represents the response returned by Ollama's /api/generate endpoint.
    /// </summary>
    public class OllamaViewModel
    {
        /// <summary>The model that generated the response.</summary>
        [JsonProperty("model")]
        public string? Model { get; set; }

        /// <summary>Timestamp when the response was created.</summary>
        [JsonProperty("created_at")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>The generated text response.</summary>
        [JsonProperty("response")]
        public string? Response { get; set; }

        /// <summary>Indicates whether generation is complete.</summary>
        [JsonProperty("done")]
        public bool Done { get; set; }

        /// <summary>Reason why generation finished (e.g., "stop").</summary>
        [JsonProperty("done_reason")]
        public string? DoneReason { get; set; }

        /// <summary>Token context used for the generation.</summary>
        [JsonProperty("context")]
        public int[]? Context { get; set; }

        /// <summary>Total duration of the request in nanoseconds.</summary>
        [JsonProperty("total_duration")]
        public long? TotalDuration { get; set; }

        /// <summary>Time spent loading the model in nanoseconds.</summary>
        [JsonProperty("load_duration")]
        public long? LoadDuration { get; set; }

        /// <summary>Number of tokens evaluated in the prompt.</summary>
        [JsonProperty("prompt_eval_count")]
        public int? PromptEvalCount { get; set; }

        /// <summary>Duration of prompt evaluation in nanoseconds.</summary>
        [JsonProperty("prompt_eval_duration")]
        public long? PromptEvalDuration { get; set; }

        /// <summary>Number of tokens generated in the response.</summary>
        [JsonProperty("eval_count")]
        public int? EvalCount { get; set; }

        /// <summary>Duration of response generation in nanoseconds.</summary>
        [JsonProperty("eval_duration")]
        public long? EvalDuration { get; set; }
    }
}
