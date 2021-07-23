using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;

namespace VMS.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository _repository;

        public ProjectService(IRepository repository)
        {
            _repository = repository;
        }

        public Task<List<Project>> GetAllProjects()
        {
            return _repository.GetListAsync<Project>();
        }
    }
}