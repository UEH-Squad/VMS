using AutoMapper;
using Microsoft.AspNetCore.Identity;
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
            CreateMap<Recruitment, ListVolunteerViewModel>();
            CreateMap<PaginatedList<Recruitment>, PaginatedList<ListVolunteerViewModel>>();
            MapAccountToUserAndBack();

            CreateMap<PaginatedList<User>, PaginatedList<UserViewModel>>();
            CreateMap<EditRequirementViewModel, Feedback>();
        }

        private void MapReportToFeedback()
        {
            CreateMap<ReportViewModel, Feedback>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.ReportBy))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.DesReport))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.ActivityId, opt => opt.MapFrom(src => src.ActivityId));
            CreateMap<Recruitment, ListVolunteerViewModel>();
        }

        private void MapAccountToUserAndBack()
        {
            PasswordHasher<User> hasher = new();
            CreateMap<CreateAccountViewModel, User>()
                .ForMember(x => x.Id, opt => opt.Ignore())
                .ForMember(x => x.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(x => x.PasswordHash, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password)
                                                                          ? hasher.HashPassword(null, src.Password)
                                                                          : hasher.HashPassword(null, src.StudentId)))
                .ForMember(x => x.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(x => x.EmailConfirmed, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.UserName)))
                .ForMember(x => x.CreatedDate, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(x => x.LockoutEnabled, opt => opt.MapFrom(src => true))
                .ForMember(x => x.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(x => x.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()));

            CreateMap<PaginatedList<User>, PaginatedList<AccountViewModel>>();
            CreateMap<User, AccountViewModel>()
                .ForMember(x => x.Faculty, opt => opt.MapFrom(src => src.FacultyId.HasValue ? src.Faculty.Name : ""));
            CreateMap<AccountViewModel, User>()
                .ForMember(x => x.NormalizedEmail, opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(x => x.NormalizedUserName, opt => opt.MapFrom(src => src.UserName.ToUpper()));
        }
    }
}