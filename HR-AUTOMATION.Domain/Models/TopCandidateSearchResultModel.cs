using HR_AUTOMATION.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class TopCandidateSearchResultModel : SearchResult
    {
        [Column("vacancy_id")]
        public int VacancyId { get; set; }
    }
}
