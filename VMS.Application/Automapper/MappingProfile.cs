using AutoMapper;
using VMS.Application.ViewModels;
using VMS.Domain.Models;

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
        }
    }
}