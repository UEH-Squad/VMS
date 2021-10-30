using AutoMapper;
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
            CreateMap<User, UserViewModel>();
            CreateMap<CreateUserProfileViewModel, User>();
            CreateMap<User, CreateUserProfileViewModel>();
            CreateMap<Faculty, FacultyViewModel>();
            CreateMap<PaginatedList<Activity>, PaginatedList<ActivityViewModel>>();
            CreateMap<ReportViewModel, Feedback>();
        }
    }
}