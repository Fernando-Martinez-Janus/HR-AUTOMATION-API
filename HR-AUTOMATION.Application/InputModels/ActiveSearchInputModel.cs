namespace HR_AUTOMATION.Application.InputModels
{
    public class ActiveSearchInputModel
    {
        public int VacancyId { get; set; }
        public int? MinExperience { get; set; }
        public int? MaxExperience { get; set; }
        public int? Education { get; set; }
        public string? CvUpdated { get; set; }
        public string? KeywordsExclude { get; set; }
        public IEnumerable<int> Sources { get; set; } = [];
    }
}