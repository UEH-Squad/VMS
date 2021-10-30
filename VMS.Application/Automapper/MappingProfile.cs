using AutoMapper;
using System;
using System.Collections.Generic;
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
            CreateMap<Activity, ViewActivityViewModel>();
            CreateMap<Area, AreaViewModel>();
            CreateMap<Activity, UserWithActivityViewModel>();
            CreateMap<Skill, SkillViewModel>();
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.Areas, opt => opt.MapFrom(src => src.UserAreas.Select(x => new AreaViewModel
                {
                    Id = x.AreaId,
                    Name = x.Area.Name,
                    Icon = x.Area.Icon,
                    Color = x.Area.Color
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
                    Icon = x.Area.Icon,
                    Color = x.Area.Color
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
                    Icon = x.Area.Icon,
                    Color = x.Area.Color
                })));
            CreateMap<Faculty, FacultyViewModel>();
            CreateMap<PaginatedList<Activity>, PaginatedList<ActivityViewModel>>();
        }
    }
}