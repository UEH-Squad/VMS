using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using VMS.Application.Interfaces;
using VMS.Application.Services;
using VMS.Domain.Configurations;
using VMS.Domain.Enums;
using VMS.Domain.Interfaces;
using VMS.Infrastructure.Data.Repositories;
using VMS.Infrastructure.Services;

namespace VMS.Infrastructure.IoC
{
    public static class DependencyContainer
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpContextAccessor();
            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));
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

            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            services.AddTransient<IRepository, Repository>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IProjectService, ProjectService>();
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPostService, PostService>();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseHangfireDashboard("/jobs");
            }
            else
            {
            }
        }
    }
}