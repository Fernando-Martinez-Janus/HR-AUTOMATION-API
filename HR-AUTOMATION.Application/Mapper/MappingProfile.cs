using AutoMapper;
using HR_AUTOMATION.Application.InputModels;
using HR_AUTOMATION.Application.ViewModels;
using HR_AUTOMATION.Domain.Models;

namespace HR_AUTOMATION.Application.Mapper
{
    /// <summary>
    /// A class that defines object-to-object mapping configurations for AutoMapper.
    /// Inherits from AutoMapper's <see cref="Profile"/> class, which provides the functionality 
    /// to define custom mappings between types.
    /// </summary>
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SkillCategoryModel, SkillCategoryViewModel>(MemberList.None)
                .ForMember(view => view.SkillCategoryId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.CategoryName, model => model.MapFrom(m => m.CategoryName))
                .ForMember(view => view.IconName, model => model.MapFrom(m => m.IconName))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<SkillModel, SkillViewModel>(MemberList.None)
                .ForMember(view => view.SkillId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.SkillCategoryId, model => model.MapFrom(m => m.SkillCategoryId))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.SkillName, model => model.MapFrom(m => m.SkillName))
                .ReverseMap();

            CreateMap<SkillLevelModel, SkillLevelViewModel>(MemberList.None)
                .ForMember(view => view.SkillLevelId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.LevelName, model => model.MapFrom(m => m.LevelName))
                .ForMember(view => view.LevelDescription, model => model.MapFrom(m => m.LevelDescription))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<AreaLevelModel, AreaLevelViewModel>(MemberList.None)
                .ForMember(view => view.AreaLevelId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.LevelName, model => model.MapFrom(m => m.LevelName))
                .ForMember(view => view.LevelDescription, model => model.MapFrom(m => m.LevelDescription))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<CriticalityLevelModel, CriticalityLevelViewModel>(MemberList.None)
                .ForMember(view => view.CriticalityLevelId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.LevelName, model => model.MapFrom(m => m.LevelName))
                .ForMember(view => view.LevelDescription, model => model.MapFrom(m => m.LevelDescription))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<RejectionReasonModel, RejectionReasonViewModel>(MemberList.None)
                .ForMember(view => view.RejectionReasonId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.Description, model => model.MapFrom(m => m.Description))
                .ForMember(view => view.IsDefinitive, model => model.MapFrom(m => m.IsDefinitive));

            CreateMap<CurrencyModel, CurrencyViewModel>(MemberList.None)
                .ForMember(view => view.CurrencyId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.CurrencyCode, model => model.MapFrom(m => m.CurrencyCode))
                .ForMember(view => view.CurrencyName, model => model.MapFrom(m => m.CurrencyName))
                .ForMember(view => view.CurrencySymbol, model => model.MapFrom(m => m.CurrencySymbol))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<PaymentPeriodModel, PaymentPeriodViewModel>(MemberList.None)
                .ForMember(view => view.PaymentPeriodId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.PeriodName, model => model.MapFrom(m => m.PeriodName))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<EmploymentTypeModel, EmploymentTypeViewModel>(MemberList.None)
                .ForMember(view => view.EmploymentTypeId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.TypeName, model => model.MapFrom(m => m.TypeName))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<WorkModalityModel, WorkModalityViewModel>(MemberList.None)
                .ForMember(view => view.WorkModalityId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.ModalityName, model => model.MapFrom(m => m.ModalityName))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<SeniorityLevelModel, SeniorityLevelViewModel>(MemberList.None)
                .ForMember(view => view.SeniorityLevelId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.SeniorityName, model => model.MapFrom(m => m.SeniorityName))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<QuestionCategoryModel, QuestionCategoryViewModel>(MemberList.None)
                .ForMember(view => view.QuestionCategoryId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.CategoryName, model => model.MapFrom(m => m.CategoryName))
                .ForMember(view => view.CategoryDescription, model => model.MapFrom(m => m.CategoryDescription));

            CreateMap<OrganizationModel, OrganizationViewModel>(MemberList.None)
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationName, model => model.MapFrom(m => m.Name))
                .ForMember(view => view.Slug, model => model.MapFrom(m => m.Slug));

            CreateMap<Vacancy, VacancyViewModel>(MemberList.None)
                .ForMember(view => view.Id, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.ProfileId, model => model.MapFrom(m => m.ProfileId))
                .ForMember(view => view.ProfileName, model => model.MapFrom(m => m.ProfileName))
                .ForMember(view => view.CriticalityLevelId, model => model.MapFrom(m => m.CriticalityLevelId))
                .ForMember(view => view.VacancyStatusId, model => model.MapFrom(m => m.VacancyStatusId))
                .ForMember(view => view.StatusName, model => model.MapFrom(m => m.StatusName))
                .ForMember(view => view.VacancyTitle, model => model.MapFrom(m => m.VacancyTitle))
                .ForMember(view => view.ClientName, model => model.MapFrom(m => m.ClientName))
                .ForMember(view => view.ProjectName, model => model.MapFrom(m => m.ProjectName))
                .ForMember(view => view.VacancyLocation, model => model.MapFrom(m => m.VacancyLocation))
                .ForMember(view => view.PositionCount, model => model.MapFrom(m => m.PositionCount))
                .ForMember(view => view.SalaryRangeMin, model => model.MapFrom(m => m.SalaryRangeMin))
                .ForMember(view => view.SalaryRangeMax, model => model.MapFrom(m => m.SalaryRangeMax))
                .ForMember(view => view.RequestDate, model => model.MapFrom(m => m.RequestDate))
                .ForMember(view => view.DeadlineDate, model => model.MapFrom(m => m.DeadlineDate))
                .ForMember(view => view.WorkModalityId, model => model.MapFrom(m => m.WorkModalityId))
                .ForMember(view => view.EmploymentTypeId, model => model.MapFrom(m => m.EmploymentTypeId))
                .ForMember(view => view.CurrencyId, model => model.MapFrom(m => m.CurrencyId))
                .ForMember(view => view.PaymentPeriodId, model => model.MapFrom(m => m.PaymentPeriodId))
                .ForMember(view => view.MinimumExperience, model => model.MapFrom(m => m.MinimumExperience))
                .ForMember(view => view.MaximumExperience, model => model.MapFrom(m => m.MaximumExperience))
                .ForMember(view => view.ScolarityId, model => model.MapFrom(m => m.ScolarityId))
                .ForMember(view => view.SkillsProfile, model => model.MapFrom(m => m.SkillsProfile))
                .ForMember(view => view.Excluded, model => model.MapFrom(m => m.Excluded))
                .ForMember(view => view.Included, model => model.MapFrom(m => m.Included))
                .ForMember(view => view.Sources, model => model.MapFrom(m => m.Sources))
                .ForMember(view => view.CvMaxAge, model => model.MapFrom(m => m.CvMaxAge))
                .ForMember(view => view.RequestCooldownMs, model => model.MapFrom(m => m.RequestCooldownMs))
                .ForMember(view => view.Notes, model => model.MapFrom(m => m.Notes))
                .ForMember(view => view.IsEnabled, model => model.MapFrom(m => m.IsEnabled));


            CreateMap<ProfileModel, ProfileViewModel>(MemberList.None)
                .ForMember(view => view.ProfileId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.AreaLevelId, model => model.MapFrom(m => m.AreaLevelId))
                .ForMember(view => view.AreaLevelName, model => model.MapFrom(m => m.AreaLevelName))
                .ForMember(view => view.SeniorityLevelId, model => model.MapFrom(m => m.SeniorityLevelId))
                .ForMember(view => view.SeniorityLevelName, model => model.MapFrom(m => m.SeniorityLevelName))
                .ForMember(view => view.ScolarityLevelId, model => model.MapFrom(m => m.ScolarityLevelId))
                .ForMember(view => view.ScolarityLevelName, model => model.MapFrom(m => m.ScolarityLevelName))
                .ForMember(view => view.MinExperience, model => model.MapFrom(m => m.MinExperience))
                .ForMember(view => view.MaxExperience, model => model.MapFrom(m => m.MaxExperience))
                .ForMember(view => view.ProfileName, model => model.MapFrom(m => m.ProfileName))
                .ForMember(view => view.ProfileDescription, model => model.MapFrom(m => m.ProfileDescription))
                .ForMember(view => view.IconName, model => model.MapFrom(m => m.IconName))
                .ForMember(view => view.Color, model => model.MapFrom(m => m.Color))
                .ForMember(view => view.Skills, model => model.MapFrom(m => m.Skills));

            CreateMap<ScolarityLevelModel, ScolarityLevelViewModel>(MemberList.None)
                .ForMember(view => view.ScolarityLevelId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.OrganizationId, model => model.MapFrom(m => m.OrganizationId))
                .ForMember(view => view.LevelName, model => model.MapFrom(m => m.LevelName))
                .ForMember(view => view.LevelDescription, model => model.MapFrom(m => m.LevelDescription))
                .ForMember(view => view.SortOrder, model => model.MapFrom(m => m.SortOrder));

            CreateMap<ProfileSkillModel, ProfileSkillViewModel>(MemberList.None)
                .ForMember(view => view.ProfileSkillId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.ProfileId, model => model.MapFrom(m => m.ProfileId))
                .ForMember(view => view.SkillId, model => model.MapFrom(m => m.SkillId))
                .ForMember(view => view.SkillName, model => model.MapFrom(m => m.SkillName))
                .ForMember(view => view.SkillLevelId, model => model.MapFrom(m => m.SkillLevelId))
                .ForMember(view => view.SkillLevelName, model => model.MapFrom(m => m.SkillLevelName))
                .ForMember(view => view.IsRequired, model => model.MapFrom(m => m.IsRequired));

            CreateMap<ProfileSkillInputModel, ProfileSkillModel>(MemberList.None)
                .ForMember(view => view.SkillId, model => model.MapFrom(m => m.SkillId))
                .ForMember(view => view.SkillLevelId, model => model.MapFrom(m => m.SkillLevelId))
                .ForMember(view => view.IsRequired, model => model.MapFrom(m => m.IsRequired))
                .ReverseMap();
            CreateMap<SearchRequestDispatchModel, SearchRequestDispatchViewModel>(MemberList.None)
                .ForMember(view => view.SearchRequestId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.VacancyId, model => model.MapFrom(m => m.VacancyId))
                .ForMember(view => view.VacancyTitle, model => model.MapFrom(m => m.VacancyTitle))
                .ForMember(view => view.ClientName, model => model.MapFrom(m => m.ClientName))
                .ForMember(view => view.ProjectName, model => model.MapFrom(m => m.ProjectName))
                .ForMember(view => view.VacancyLocation, model => model.MapFrom(m => m.VacancyLocation))
                .ForMember(view => view.DeadlineDate, model => model.MapFrom(m => m.DeadlineDate))
                .ForMember(view => view.CriticalityLevelId, model => model.MapFrom(m => m.CriticalityLevelIdSp))
                .ForMember(view => view.CriticalityLevelName, model => model.MapFrom(m => m.CriticalityLevelName))
                .ForMember(view => view.CriticalitySortOrder, model => model.MapFrom(m => m.CriticalitySortOrder))
                .ForMember(view => view.ProfileId, model => model.MapFrom(m => m.ProfileId))
                .ForMember(view => view.ProfileName, model => model.MapFrom(m => m.ProfileName))
                .ForMember(view => view.SeniorityLevelId, model => model.MapFrom(m => m.SeniorityLevelId))
                .ForMember(view => view.SeniorityLevelName, model => model.MapFrom(m => m.SeniorityLevelName))
                .ForMember(view => view.AreaLevelId, model => model.MapFrom(m => m.AreaLevelId))
                .ForMember(view => view.AreaLevelName, model => model.MapFrom(m => m.AreaLevelName))
                .ForMember(view => view.WorkModality, model => model.MapFrom(m => m.WorkModality))
                .ForMember(view => view.EmploymentType, model => model.MapFrom(m => m.EmploymentType))
                .ForMember(view => view.SalaryRangeMin, model => model.MapFrom(m => m.SalaryRangeMin))
                .ForMember(view => view.SalaryRangeMax, model => model.MapFrom(m => m.SalaryRangeMax))
                .ForMember(view => view.MaxProfileAgeDays, model => model.MapFrom(m => m.MaxProfileAgeDays))
                .ForMember(view => view.RequestCooldownMs, model => model.MapFrom(m => m.RequestCooldownMs))
                .ForMember(view => view.MinimumExperience, model => model.MapFrom(m => m.MinimumExperience))
                .ForMember(view => view.MaximumExperience, model => model.MapFrom(m => m.MaximumExperience))
                .ForMember(view => view.ScolarityId, model => model.MapFrom(m => m.ScolarityId))
                .ForMember(view => view.ScolarityName, model => model.MapFrom(m => m.ScolarityName))
                .ForMember(view => view.Excluded, model => model.MapFrom(m => m.Excluded))
                .ForMember(view => view.Included, model => model.MapFrom(m => m.Included))
                .ForMember(view => view.Sources, opt => opt.Ignore())
                .ForMember(view => view.SearchStatus, model => model.MapFrom(m => m.SearchStatus))
                .ForMember(view => view.CreatedAt, model => model.MapFrom(m => m.CreatedAt))
                .ForMember(view => view.PreviousCandidates, opt => opt.Ignore())
                .ForMember(view => view.SkillsProfile, opt => opt.Ignore());

            CreateMap<SearchRequestModel, SearchRequestViewModel>(MemberList.None)
                .ForMember(view => view.SearchRequestId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.VacancyId, model => model.MapFrom(m => m.VacancyId))
                .ForMember(view => view.VacancyTitle, model => model.MapFrom(m => m.VacancyTitle))
                .ForMember(view => view.ClientName, model => model.MapFrom(m => m.ClientName))
                .ForMember(view => view.VacancyLocation, model => model.MapFrom(m => m.VacancyLocation))
                .ForMember(view => view.MinimumExperience, model => model.MapFrom(m => m.MinimumExperience))
                .ForMember(view => view.MaximumExperience, model => model.MapFrom(m => m.MaximumExperience))
                .ForMember(view => view.ScolarityId, model => model.MapFrom(m => m.ScolarityId))
                .ForMember(view => view.ScolarityName, model => model.MapFrom(m => m.ScolarityName))
                .ForMember(view => view.Excluded, model => model.MapFrom(m => m.Excluded))
                .ForMember(view => view.Included, model => model.MapFrom(m => m.Included))
                .ForMember(view => view.SearchStatus, model => model.MapFrom(m => m.SearchStatus))
                .ForMember(view => view.TotalRecords, model => model.MapFrom(m => m.TotalRecords));

            CreateMap<SearchResultModel, SearchResultsViewModel>(MemberList.None)
                .ForMember(view => view.SearchResultId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.SearchRequestId, model => model.MapFrom(m => m.SearchRequestId))
                .ForMember(view => view.CandidateName, model => model.MapFrom(m => m.CandidateName))
                .ForMember(view => view.CandidateTitle, model => model.MapFrom(m => m.CandidateTitle))
                .ForMember(view => view.Experience, model => model.MapFrom(m => m.Experience))
                .ForMember(view => view.CurrentCompany, model => model.MapFrom(m => m.CurrentCompany))
                .ForMember(view => view.Location, model => model.MapFrom(m => m.Location))
                .ForMember(view => view.Email, model => model.MapFrom(m => m.Email))
                .ForMember(view => view.Phone, model => model.MapFrom(m => m.Phone))
                .ForMember(view => view.Source, model => model.MapFrom(m => m.Source))
                // Mapeo explicito por diferencia de prefijo (Ai vs Ia) según la interfaz TypeScript
                .ForMember(view => view.AiScore, model => model.MapFrom(m => m.IaScore))
                .ForMember(view => view.AiRecommended, model => model.MapFrom(m => m.IaRecommended))
                .ForMember(view => view.AiShortComment, model => model.MapFrom(m => m.IaShortComment))
                .ForMember(view => view.AiExtendedComment, model => model.MapFrom(m => m.IaExtendedComment))
                .ForMember(view => view.ReferenceLink, model => model.MapFrom(m => m.ReferenceLink))
                .ForMember(view => view.OriginalResumeLink, model => model.MapFrom(m => m.OriginalResumeLink))
                .ForMember(view => view.Relocation, model => model.MapFrom(m => m.Relocation))
                .ForMember(view => view.Seen, model => model.MapFrom(m => m.Seen))
                .ForMember(view => view.IsEnabled, model => model.MapFrom(m => m.IsEnabled))
                .ForMember(view => view.CreatedAt, model => model.MapFrom(m => m.CreatedAt))
                .ForMember(view => view.CreatedBy, model => model.MapFrom(m => m.CreatedBy))
                .ForMember(view => view.UpdatedAt, model => model.MapFrom(m => m.UpdatedAt))
                .ForMember(view => view.UpdatedBy, model => model.MapFrom(m => m.UpdatedBy));

            CreateMap<SearchResultModel, SearchResultViewModel>(MemberList.None)
                .ForMember(view => view.SearchResultId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.SearchRequestId, model => model.MapFrom(m => m.SearchRequestId))
                .ForMember(view => view.CandidateName, model => model.MapFrom(m => m.CandidateName))
                .ForMember(view => view.CandidateTitle, model => model.MapFrom(m => m.CandidateTitle))
                .ForMember(view => view.Experience, model => model.MapFrom(m => m.Experience))
                .ForMember(view => view.CurrentCompany, model => model.MapFrom(m => m.CurrentCompany))
                .ForMember(view => view.Location, model => model.MapFrom(m => m.Location))
                .ForMember(view => view.Email, model => model.MapFrom(m => m.Email))
                .ForMember(view => view.Phone, model => model.MapFrom(m => m.Phone))
                .ForMember(view => view.Source, model => model.MapFrom(m => m.Source))
                .ForMember(view => view.Preferences, model => model.MapFrom(m =>
                    string.IsNullOrWhiteSpace(m.RawPreferences)
                        ? new List<string>()
                        : m.RawPreferences.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList()))
                .ForMember(view => view.AiScore, model => model.MapFrom(m => m.IaScore ?? 0))
                .ForMember(view => view.AiRecommended, model => model.MapFrom(m => m.IaRecommended))
                .ForMember(view => view.AiShortComment, model => model.MapFrom(m => m.IaShortComment))
                .ForMember(view => view.AiExtendedComment, model => model.MapFrom(m => m.IaExtendedComment))
                .ForMember(view => view.ReferenceLink, model => model.MapFrom(m => m.ReferenceLink))
                .ForMember(view => view.OriginalResumeLink, model => model.MapFrom(m => m.OriginalResumeLink))
                .ForMember(view => view.Relocation, model => model.MapFrom(m => m.Relocation))
                .ForMember(view => view.Seen, model => model.MapFrom(m => m.Seen))
                .ForMember(view => view.CandidateRank, model => model.MapFrom(m => m.CandidateRank))
                .ForMember(view => view.CreatedAt, model => model.MapFrom(m => m.CreatedAt));

            CreateMap<TopCandidateSearchResultModel, SearchResultViewModel>(MemberList.None)
                .ForMember(view => view.SearchResultId, model => model.MapFrom(m => m.Id))
                .ForMember(view => view.SearchRequestId, model => model.MapFrom(m => m.SearchRequestId))
                .ForMember(view => view.VacancyId, model => model.MapFrom(m => m.VacancyId))
                .ForMember(view => view.CandidateName, model => model.MapFrom(m => m.CandidateName))
                .ForMember(view => view.CandidateTitle, model => model.MapFrom(m => m.CandidateTitle))
                .ForMember(view => view.Experience, model => model.MapFrom(m => m.Experience))
                .ForMember(view => view.CurrentCompany, model => model.MapFrom(m => m.CurrentCompany))
                .ForMember(view => view.Location, model => model.MapFrom(m => m.Location))
                .ForMember(view => view.Email, model => model.MapFrom(m => m.Email))
                .ForMember(view => view.Phone, model => model.MapFrom(m => m.Phone))
                .ForMember(view => view.Source, model => model.MapFrom(m => m.Source))
                .ForMember(view => view.Preferences, model => model.MapFrom(m =>
                    string.IsNullOrWhiteSpace(m.RawPreferences)
                        ? new List<string>()
                        : m.RawPreferences.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList()))
                .ForMember(view => view.AiScore, model => model.MapFrom(m => m.IaScore ?? 0))
                .ForMember(view => view.AiRecommended, model => model.MapFrom(m => m.IaRecommended))
                .ForMember(view => view.AiShortComment, model => model.MapFrom(m => m.IaShortComment))
                .ForMember(view => view.AiExtendedComment, model => model.MapFrom(m => m.IaExtendedComment))
                .ForMember(view => view.ReferenceLink, model => model.MapFrom(m => m.ReferenceLink))
                .ForMember(view => view.OriginalResumeLink, model => model.MapFrom(m => m.OriginalResumeLink))
                .ForMember(view => view.Relocation, model => model.MapFrom(m => m.Relocation))
                .ForMember(view => view.Seen, model => model.MapFrom(m => m.Seen))
                .ForMember(view => view.CandidateRank, model => model.MapFrom(m => m.CandidateRank))
                .ForMember(view => view.CreatedAt, model => model.MapFrom(m => m.CreatedAt));
        }
    }
}