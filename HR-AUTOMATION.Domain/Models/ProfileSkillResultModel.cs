using System.ComponentModel.DataAnnotations.Schema;

namespace HR_AUTOMATION.Domain.Models
{
    public class ProfileSkillResultModel
    {
        [Column("skill_id")]
        public int SkillId { get; set; }

        [Column("skill_category_id")]
        public int SkillCategoryId { get; set; }

        [Column("skill_level_id")]
        public int SkillLevelId { get; set; }
    }
}
