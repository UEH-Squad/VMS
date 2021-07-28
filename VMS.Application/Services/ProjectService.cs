using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class ProjectService : BaseService, IProjectService
    {
        private readonly IIdentityService _userService;

        public ProjectService(IRepository repository,
                              IDbContextFactory<VmsDbContext> dbContextFactory,
                              IIdentityService userService)
            : base(repository, dbContextFactory)
        {
            _userService = userService;
        }

        public async Task<List<ProjectViewModel>> GetAllProjects()
        {
            Specification<Project> specification = new()
            {
                Includes = project => project.Include(x => x.Category).Include(x => x.Organization)
            };

            DbContext context = _dbContextFactory.CreateDbContext();

            List<Project> projects = await _repository.GetListAsync(context, specification);

            List<ProjectViewModel> projectVM = new();
            foreach (var project in projects)
            {
                ProjectViewModel viewModel = new()
                {
                    Name = project.Name,
                    Category = project.Category.Name
                };

                viewModel.Organization = _userService.FindUserById(project.OrganizationId)?.UserName;

                projectVM.Add(viewModel);
            }

            return projectVM;
        }

        public async Task<int> CreateProject(CreateProjectViewModel project)
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            IdentityUser currentUser = _userService.GetCurrentUser();
            Project projectMapping = new()
            {
                Name = project.Name,
                ShortDescription = project.ShortDescription,
                CategoryId = project.CategoryId,
                OrganizationId = currentUser.Id
            };

            object[] newId = await _repository.InsertAsync(context, projectMapping);
            return (int)newId[0];
        }
    }
}