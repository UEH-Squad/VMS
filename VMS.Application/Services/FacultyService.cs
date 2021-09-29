using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class FacultyService : BaseService, IFacultyService
    {
        public FacultyService(IRepository repository,
                              IDbContextFactory<VmsDbContext> dbContextFactory,
                              IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<FacultyViewModel>> GetAllFacultiesAsync()
        {
            using DbContext dbContext = _dbContextFactory.CreateDbContext();
            List<Faculty> faculties = await _repository.GetListAsync<Faculty>(dbContext);
            return _mapper.Map<List<FacultyViewModel>>(faculties);
        }
    }
}
