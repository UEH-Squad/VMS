﻿using AutoMapper;
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
            CreateMap<Activity, ActivityViewModel>()
                .ForMember(x => x.MemberQuantity, opt => opt.MapFrom(src => src.Recruitments.Count));
            CreateMap<CreateActivityViewModel, Activity>()
                .ForMember(dest => dest.OpenDate, opt => opt.MapFrom(src => src.OpenDate.Date))
                .ForMember(dest => dest.CloseDate, opt => opt.MapFrom(src => src.CloseDate.Date))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate.Date))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate.Date));
            CreateMap<Activity, CreateActivityViewModel>();
            CreateMap<Activity, ViewActivityViewModel>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => src.CreatedDate.ToString("dd/MM/yyyy")));
            CreateMap<Area, AreaViewModel>();
            CreateMap<Activity, UserWithActivityViewModel>();
            CreateMap<Skill, SkillViewModel>();
            CreateMap<User, UserViewModel>()
                .ForMember(x => x.Areas, opt => opt.MapFrom(src => src.UserAreas.OrderByDescending(x => x.Area.IsPinned).Select(x => new AreaViewModel
                {
                    Id = x.AreaId,
                    Name = x.Area.Name,
                    Icon = x.Area.Icon,
                    Color = x.Area.Color,
                    IsPinned = x.Area.IsPinned
                })))
                .ForMember(x => x.Skills, opt => opt.MapFrom(src => src.UserSkills.Select(x => new SkillViewModel
                {
                    Id = x.SkillId,
                    Name = x.Skill.Name
                })));
            CreateMap<CreateUserProfileViewModel, User>();
            CreateMap<CreateUserProfileViewModel, User>();
            CreateMap<User, CreateUserProfileViewModel>()
                .ForMember(x => x.Areas, opt => opt.MapFrom(src => src.UserAreas.OrderByDescending(x => x.Area.IsPinned).Select(x => new AreaViewModel
                {
                    Id = x.AreaId,
                    Name = x.Area.Name,
                    Icon = x.Area.Icon,
                    Color = x.Area.Color,
                    IsPinned = x.Area.IsPinned
                })))
                .ForMember(x => x.Skills, opt => opt.MapFrom(src => src.UserSkills.Select(x => new SkillViewModel
                {
                    Id = x.SkillId,
                    Name = x.Skill.Name
                })));

            CreateMap<CreateOrgProfileViewModel, User>();
            CreateMap<User, CreateOrgProfileViewModel>()
                .ForMember(x => x.Areas, opt => opt.MapFrom(src => src.UserAreas.OrderByDescending(x => x.Area.IsPinned).Select(x => new AreaViewModel
                {
                    Id = x.AreaId,
                    Name = x.Area.Name,
                    Icon = x.Area.Icon,
                    Color = x.Area.Color,
                    IsPinned = x.Area.IsPinned
                })));
            CreateMap<Faculty, FacultyViewModel>();
            CreateMap<PaginatedList<Activity>, PaginatedList<ActivityViewModel>>();
            MapReportToFeedback();

            CreateMap<Recruitment, RecruitmentViewModel>()
                .ForMember(x => x.Organizer, opt => opt.MapFrom(src => src.Activity.Organizer));
            CreateMap<PaginatedList<Recruitment>, PaginatedList<RecruitmentViewModel>>();
            CreateMap<RecruitmentRating, RecruitmentRatingViewModel>();
        }

        private void MapReportToFeedback()
        {
            CreateMap<ReportViewModel, Feedback>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.ReportBy))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.DesReport))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId));
        }
    }
}