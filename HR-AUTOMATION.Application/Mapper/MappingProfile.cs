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
        }
    }
}