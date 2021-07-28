using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.ViewModels;

namespace VMS.Application.Interfaces
{
    public interface IProjectService
    {
        Task<List<ProjectViewModel>> GetAllProjects();

        Task<int> CreateProject(CreateProjectViewModel project);
    }
}