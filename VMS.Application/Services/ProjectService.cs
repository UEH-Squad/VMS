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

namespace VMS.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository _repository;
        private readonly UserManager<IdentityUser> _usermanager;
        private readonly IHttpContextAccessor _httpContext;

        public ProjectService(IRepository repository,
                              UserManager<IdentityUser> userManager,
                              IHttpContextAccessor httpContext)
        {
            _repository = repository;
            _usermanager = userManager;
            _httpContext = httpContext;
        }

        public async Task<List<ProjectViewModel>> GetAllProjects()
        {
            Specification<Project> specification = new()
            {
                Includes = project => project.Include(x => x.Category).Include(x => x.Organization)
            };

            List<Project> projects = await _repository.GetListAsync(specification);

            List<ProjectViewModel> projectVM = new();
            foreach (var project in projects)
            {
                ProjectViewModel viewModel = new()
                {
                    Name = project.Name,
                    Category = project.Category.Name
                };

                viewModel.Organization = (await _usermanager.FindByIdAsync(project.OrganizationId))?.UserName;

                projectVM.Add(viewModel);
            }

            return projectVM;
        }

        public async Task<int> CreateProject(CreateProjectViewModel project)
        {
            IdentityUser currentUser = await _usermanager.GetUserAsync(_httpContext.HttpContext.User);
            var projectMapping = new Project()
            {
                Name = project.Name,
                ShortDescription = project.ShortDescription,
                CategoryId = project.CategoryId,
                OrganizationId = currentUser.Id
            };

            var newId = await _repository.InsertAsync<Project>(projectMapping);
            return (int)newId[0];
        }
    }
}