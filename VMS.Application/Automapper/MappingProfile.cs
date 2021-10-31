using AutoMapper;
using System;
using System.Linq;
using VMS.Application.ViewModels;
using VMS.Domain.Models;
using VMS.GenericRepository;

namespace VMS.Application.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activity, ActivityViewModel>();
            CreateMap<CreateActivityViewModel, Activity>();
            CreateMap<Activity, CreateActivityViewModel>();
            CreateMap<Activity, ViewActivityViewModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.ToString("dd/MM/yyyy")))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.ToString("dd/MM/yyyy")));
            CreateMap<Area, AreaViewModel>();
            CreateMap<Activity, UserWithActivityViewModel>();
            CreateMap<Skill, SkillViewModel>();
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.Areas, opt => opt.MapFrom(src => src.UserAreas.Select(x => new AreaViewModel
                {
                    Id = x.AreaId,
                    Name = x.Area.Name,
                    Icon = x.Area.Icon
                })))
                .ForMember(x => x.Skills, opt => opt.MapFrom(src => src.UserSkills.Select(x => new SkillViewModel
                {
                    Id = x.SkillId,
                    Name = x.Skill.Name
                })));
            CreateMap<CreateUserProfileViewModel, User>();
            CreateMap<CreateUserProfileViewModel, User>();
            CreateMap<User, CreateUserProfileViewModel>()
                .ForMember(x => x.Areas, opt => opt.MapFrom(src => src.UserAreas.Select(x => new AreaViewModel
                {
                    Id = x.AreaId,
                    Name = x.Area.Name,
                    Icon = x.Area.Icon
                })))
                .ForMember(x => x.Skills, opt => opt.MapFrom(src => src.UserSkills.Select(x => new SkillViewModel
                {
                    Id = x.SkillId,
                    Name = x.Skill.Name
                })));

            CreateMap<CreateOrgProfileViewModel, User>();
            CreateMap<User, CreateOrgProfileViewModel>()
                .ForMember(x => x.Areas, opt => opt.MapFrom(src => src.UserAreas.Select(x => new AreaViewModel
                {
                    Id = x.AreaId,
                    Name = x.Area.Name,
                    Icon = x.Area.Icon
                })));
            CreateMap<Faculty, FacultyViewModel>();
            CreateMap<PaginatedList<Activity>, PaginatedList<ActivityViewModel>>();
            MapReportToFeedback();
        }

        private void MapReportToFeedback() {
            CreateMap<ReportViewModel, Feedback>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.DesReport))
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId));
        }
    }
}