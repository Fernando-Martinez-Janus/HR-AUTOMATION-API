using System.ComponentModel.DataAnnotations.Schema;
using HR_AUTOMATION.Domain.Entities;

namespace HR_AUTOMATION.Domain.Models
{
    public class SearchRequestModel : SearchRequest
    {
        [Column("vacancy_title")]
        public string? VacancyTitle { get; set; }

        [Column("client_name")]
        public string? ClientName { get; set; }

        [Column("vacancy_location")]
        public string? VacancyLocation { get; set; }

        [Column("scolarity_name")]
        public string? ScolarityName { get; set; }

        [Column("scolarity_id")]
        public int? ScolarityId { get; set; }

        [Column("minimum_experience")]
        public int? MinimumExperience { get; set; }

        [Column("maximum_experience")]
        public int? MaximumExperience { get; set; }

        [Column("skills_profile")]
        public string? SkillsProfile { get; set; }

        [Column("excluded")]
        public string? Excluded { get; set; }

        [Column("included")]
        public string? Included { get; set; }

        [Column("sources")]
        public string? Sources { get; set; }

        [Column("total_records")]
        public int TotalRecords { get; set; }
    }
}
