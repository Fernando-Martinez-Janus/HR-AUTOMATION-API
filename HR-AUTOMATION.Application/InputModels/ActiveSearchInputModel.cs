namespace HR_AUTOMATION.Application.InputModels
{
    public class ActiveSearchInputModel
    {
        public decimal? MinExperience { get; set; }
        public decimal? MaxExperience { get; set; }
        public int? Education { get; set; }
        public string? CvUpdated { get; set; }
        public string? KeywordsExclude { get; set; }
        public string? Included { get; set; }
        public int? CvMaxAge { get; set; }
        public int? RequestCooldownMs { get; set; }
        public IEnumerable<int> Sources { get; set; } = [];
    }
}