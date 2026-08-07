using HR_AUTOMATION.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class SearchResultModel : SearchResult
    {
        // NO vuelvas a declarar Id, SearchRequestId, CandidateTitle, etc.
        // Ya las heredaste de SearchResult.

        // Solo agrega lo que NO está en la entidad base:
        [Column("total_records")]
        public int TotalRecords { get; set; }

        [Column("unseen_count")]
        public int UnseenCount { get; set; }
    }
}