using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Entities
{
    public class SearchRequest
    {
        [Column("search_request_id")]
        public int Id { get; set; }

        [Column("vacancy_id")]
        public int VacancyId { get; set; }

        [Column("minimum_experience")]
        public int? MinimumExperience { get; set; }

        [Column("maximum_experience")]
        public int? MaximumExperience { get; set; }

        [Column("scolarity_id")]
        public int? ScolarityId { get; set; }

        [Column("skills_profile")]
        public string? SkillsProfile { get; set; }

        [Column("excluded")]
        public string? Excluded { get; set; }

        [Column("included")]
        public string? Included { get; set; }

        [Column("sources")]
        public string? Sources { get; set; }

        [Column("search_status")]
        public string SearchStatus { get; set; } = null!;

        [Column("cv_max_age")]
        public int? MaxProfileAgeDays { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("created_by")]
        public int CreatedBy { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }
    }
}
