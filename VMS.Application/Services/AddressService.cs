using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class AddressService : BaseService, IAddressService
    {
        public AddressService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<AddressPath>> GetAllProvincesAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<AddressPath> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<AddressPath, bool>>>()
                {
                    a => a.AddressPathType.Type == "Tỉnh" || a.AddressPathType.Type == "Thành Phố Trung Ương"
                }
            };

            List<AddressPath> addressPaths = await _repository.GetListAsync(dbContext, specification);

            return addressPaths;
        }

        public async Task<List<AddressPath>> GetAllDistrictsByParentIdAsync(int parentId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<AddressPath> specification = new()
            {
                Conditions = new List<System.Linq.Expressions.Expression<Func<AddressPath, bool>>>()
                {
                    a => a.ParentPathId == parentId
                }
            };

            List<AddressPath> addressPaths = await _repository.GetListAsync(dbContext, specification);

            return addressPaths;
        }
    }
}
