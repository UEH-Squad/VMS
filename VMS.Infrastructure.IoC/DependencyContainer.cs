using Microsoft.Extensions.DependencyInjection;
using System;
using VMS.Application.Interfaces;
using VMS.Application.Services;
using VMS.Domain.Enums;
using VMS.Domain.Interfaces;
using VMS.Infrastructure.Data.Repositories;
using VMS.Infrastructure.Services;

namespace VMS.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddTransient<MemoryCacheService>();
            services.AddTransient<RedisCacheService>();

            services.AddTransient<Func<CacheTech, ICacheService>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case CacheTech.Redis:
                        serviceProvider.GetService<RedisCacheService>();
                        break;

                    case CacheTech.Memory:
                        serviceProvider.GetService<MemoryCacheService>();
                        break;
                }

                return serviceProvider.GetService<MemoryCacheService>();
            });

            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IProjectService, ProjectService>();
        }
    }
}