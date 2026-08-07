using HR_AUTOMATION.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int? MaxProfileAgeDays { get; set; }

        [Column("request_cooldown_ms")]
        public int RequestCooldownMs { get; set; }

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
        public string? SearchStatus { get; set; }

        [Column("total_records")]
        public int TotalRecords { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("previous_candidates")]
        public string? PreviousCandidates { get; set; }

        [Column("max_cvs")]
        public int? MaxCvs { get; set; }

        [Column("min_match_score")]
        public int? MinMatchScore { get; set; }
    }
}
