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
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class AreaService : BaseService, IAreaService
    {
        public AreaService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<AreaViewModel>> GetAllAreas()
        {
            DbContext context = _dbContextFactory.CreateDbContext();
            List<Area> areas = await _repository.GetListAsync<Area>(context);

            //List<AreaViewModel> areasViewModel = _mapper.Map<List<AreaViewModel>>(areas);
            //return areasViewModel;

            IEnumerable<AreaViewModel> Areas = areas.Select(x => new AreaViewModel
            {
                Name = x.Name,
                Icon = x.Icon
            });

            return Areas.ToList();

        }      

        

}
}
