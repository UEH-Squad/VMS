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
            CreateMap<Activity, ActivityViewModel>()
                .ForMember(a => a.Coordinate, a => a
                .MapFrom(x => new Coordinate() { Latitude = x.Latitude, Longitude = x.Longitude }));
            CreateMap<CreateActivityViewModel, Activity>();
            CreateMap<Activity, CreateActivityViewModel>();
            CreateMap<Activity, ViewActivityViewModel>();
            CreateMap<Area, AreaViewModel>();
            CreateMap<Activity, UserWithActivityViewModel>();
            CreateMap<Skill, SkillViewModel>();
            CreateMap<PaginatedList<Activity>, PaginatedList<ActivityViewModel>>();
        }
    }
}