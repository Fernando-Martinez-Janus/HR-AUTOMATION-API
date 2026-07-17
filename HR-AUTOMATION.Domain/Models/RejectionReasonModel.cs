using HR_AUTOMATION.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class RejectionReasonModel : RejectionReason
    {
        [Column("total_count")]
        public int TotalCount { get; set; }
    }
}