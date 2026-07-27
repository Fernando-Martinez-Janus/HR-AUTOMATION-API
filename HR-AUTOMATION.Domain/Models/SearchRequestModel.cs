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

        [Column("total_records")]
        public int TotalRecords { get; set; }
    }
}
