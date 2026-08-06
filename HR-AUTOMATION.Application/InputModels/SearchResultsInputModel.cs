using System.Text.Json.Serialization;

namespace HR_AUTOMATION.Application.InputModels
{
    public class SearchResultsInputModel
    {
        public int? OrganizationId { get; set; }

        public int SearchRequestId { get; set; }

        public string? LinkPerfil { get; set; }

        /// <summary>
        /// Representa el array de candidatos o el JSON que se enviará al Stored Procedure.
        /// </summary>
        public List<SearchResultCandidateInputModel>? Candidates { get; set; }

        /// <summary>
        /// Opcional: Si el scraper o cliente ya envía el string JSON directamente formateado.
        /// </summary>
        public string? JsonQuery { get; set; }

        public void Normalize()
        {
            LinkPerfil = LinkPerfil?.Trim();
            JsonQuery = JsonQuery?.Trim();

            if (Candidates != null)
            {
                foreach (var candidate in Candidates)
                {
                    candidate.Normalize();
                }
            }
        }
    }

    public class SearchResultCandidateInputModel
    {
        [JsonPropertyName("CandidateTitle")]
        public string? CandidateTitle { get; set; }

        [JsonPropertyName("AiScore")]
        public int? AiScore { get; set; }

        [JsonPropertyName("AiShortComment")]
        public string? AiShortComment { get; set; }

        [JsonPropertyName("AiExtendedComment")]
        public string? AiExtendedComment { get; set; }

        [JsonPropertyName("ReferenceLink")]
        public string? ReferenceLink { get; set; }

        [JsonPropertyName("OriginalResumeLink")]
        public string? OriginalResumeLink { get; set; }

        public void Normalize()
        {
            CandidateTitle = CandidateTitle?.Trim();
            AiShortComment = AiShortComment?.Trim();
            AiExtendedComment = AiExtendedComment?.Trim();
            ReferenceLink = ReferenceLink?.Trim();
            OriginalResumeLink = OriginalResumeLink?.Trim();
        }
    }
}