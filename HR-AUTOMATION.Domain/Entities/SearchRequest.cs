using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class SearchRequest
    {
        [Column("search_request_id")]
        public int Id { get; set; }

        [Column("vacancy_id")]
        public int VacancyId { get; set; }

        [Column("search_status")]
        public string SearchStatus { get; set; } = "finished";

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }
    }
}
