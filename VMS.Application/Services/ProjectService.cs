using System.Collections.Generic;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using VMS.Application.Interfaces;
using VMS.Domain.Models;

namespace VMS.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository repository;

        public ProjectService(IRepository repository)
        {
            this.repository = repository;
        }

        public Task<List<Project>> GetAllProjects()
        {
            return repository.GetListAsync<Project>();
        }
    }
}