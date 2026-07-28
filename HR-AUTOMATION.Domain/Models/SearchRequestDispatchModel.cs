using System.ComponentModel.DataAnnotations.Schema;
using HR_AUTOMATION.Domain.Entities;

namespace HR_AUTOMATION.Domain.Models
{
    public class SearchRequestDispatchModel : SearchRequest
    {
        [Column("vacancy_title")]
        public string? VacancyTitle { get; set; }

        [Column("client_name")]
        public string? ClientName { get; set; }

        [Column("project_name")]
        public string? ProjectName { get; set; }

        [Column("vacancy_location")]
        public string? VacancyLocation { get; set; }

        [Column("deadline_date")]
        public DateTime? DeadlineDate { get; set; }

        [Column("criticality_level_id")]
        public int? CriticalityLevelIdSp { get; set; }

        [Column("criticality_level_name")]
        public string? CriticalityLevelName { get; set; }

        [Column("criticality_sort_order")]
        public int? CriticalitySortOrder { get; set; }

        [Column("profile_id")]
        public int ProfileId { get; set; }

        [Column("profile_name")]
        public string? ProfileName { get; set; }

        [Column("scolarity_name")]
        public string? ScolarityName { get; set; }

        [Column("seniority_level_id")]
        public int? SeniorityLevelId { get; set; }

        [Column("seniority_level_name")]
        public string? SeniorityLevelName { get; set; }

        [Column("area_level_id")]
        public int? AreaLevelId { get; set; }

        [Column("area_level_name")]
        public string? AreaLevelName { get; set; }

        [Column("work_modality")]
        public string? WorkModality { get; set; }

        [Column("employment_type")]
        public string? EmploymentType { get; set; }

        [Column("salary_range_min")]
        public decimal SalaryRangeMin { get; set; }

        [Column("salary_range_max")]
        public decimal SalaryRangeMax { get; set; }

        [Column("max_profile_age_days")]
        public int MaxProfileAgeDays { get; set; }

        [Column("request_cooldown_ms")]
        public int RequestCooldownMs { get; set; }

        [Column("previous_candidates")]
        public string? PreviousCandidates { get; set; }
    }
}
