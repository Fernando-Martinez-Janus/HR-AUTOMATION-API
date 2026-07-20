using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class Vacancy
    {
        [Column("vacancy_id")]
        public int Id { get; set; }

        [Column("organization_id")]
        public int OrganizationId { get; set; }

        [Column("profile_id")]
        public int ProfileId { get; set; }

        [Column("criticality_level_id")]
        public int CriticalityLevelId { get; set; }

        [Column("vacancy_status_id")]
        public int VacancyStatusId { get; set; }

        [Column("vacancy_title")]
        public string VacancyTitle { get; set; } = string.Empty;

        [Column("client_name")]
        public string ClientName { get; set; } = string.Empty;

        [Column("project_name")]
        public string ProjectName { get; set; } = string.Empty;

        [Column("vacancy_location")]
        public string VacancyLocation { get; set; } = string.Empty;

        [Column("position_count")]
        public int PositionCount { get; set; }

        [Column("salary_range_min")]
        public decimal SalaryRangeMin { get; set; }

        [Column("salary_range_max")]
        public decimal SalaryRangeMax { get; set; }

        [Column("request_date")]
        public DateTime RequestDate { get; set; }

        [Column("deadline_date")]
        public DateTime? DeadlineDate { get; set; }

        [Column("work_modality_id")]
        public int? WorkModalityId { get; set; }

        [Column("employment_type_id")]
        public int? EmploymentTypeId { get; set; }

        [Column("currency_id")]
        public int? CurrencyId { get; set; }

        [Column("payment_period_id")]
        public int? PaymentPeriodId { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("profile_name")]
        public string? ProfileName { get; set; }

        [Column("status_name")]
        public string? StatusName { get; set; }

        [Column("is_enabled")]
        public bool IsEnabled { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        [Column("created_by")]
        public int? CreatedBy { get; set; }

        [Column("updated_by")]
        public int? UpdatedBy { get; set; }

        [Column("total_records")]
        public int TotalRecords { get; set; }
    }
}
