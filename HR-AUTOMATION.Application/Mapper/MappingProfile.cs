using AutoMapper;
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
                .ForMember(view => view.SkillName, model => model.MapFrom(m => m.SkillName));

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
                .ForMember(view => view.ModalityId, model => model.MapFrom(m => m.ModalityId))
                .ForMember(view => view.ContractTypeId, model => model.MapFrom(m => m.ContractTypeId))
                .ForMember(view => view.CurrencyId, model => model.MapFrom(m => m.CurrencyId))
                .ForMember(view => view.PayFrequencyId, model => model.MapFrom(m => m.PayFrequencyId))
                .ForMember(view => view.Notes, model => model.MapFrom(m => m.Notes))
                .ForMember(view => view.IsEnabled, model => model.MapFrom(m => m.IsEnabled));
        }
    }
}