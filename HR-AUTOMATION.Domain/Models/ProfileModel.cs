using HR_AUTOMATION.Domain.Entities;
using HR_AUTOMATION.Domain.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class ProfileModel : Profile
{
    // Sobrescribe las de la clase base para que Dapper las vea aquí
    [Column("area_level_id")]
    public new int AreaLevelId { get; set; }

    [Column("seniority_level_id")]
    public new int SeniorityLevelId { get; set; }

    [Column("scolarity_level_id")]
    public new int? ScolarityLevelId { get; set; }

    // Nombres de levels (ya los tienes, pero asegúrate de que coincidan)
    [Column("area_level_name")]
    public string? AreaLevelName { get; set; }

    [Column("seniority_level_name")]
    public string? SeniorityLevelName { get; set; }

    [Column("scolarity_level_name")]
    public string? ScolarityLevelName { get; set; }

    [Column("profile_name")]
    public new string? ProfileName { get; set; }

    [Column("skills")]
    public new IEnumerable<ProfileSkillModel> Skills { get; set; } = [];

    [Column("total_count")]
    public int TotalCount { get; set; }
}