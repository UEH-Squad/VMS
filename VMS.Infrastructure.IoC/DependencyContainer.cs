using Microsoft.Extensions.DependencyInjection;
using TanvirArjel.EFCore.GenericRepository;
using VMS.Application.Interfaces;
using VMS.Application.Services;
using VMS.Infrastructure.Data.Context;

namespace VMS.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddGenericRepository<VmsDbContext>();
            services.AddTransient<IProjectService, ProjectService>();
        }
    }
}