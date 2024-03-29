﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        public AddressService(IRepository repository,
                              IDbContextFactory<VmsDbContext> dbContextFactory,
                              IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<List<AddressPath>> GetAllProvincesAsync()
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<AddressPath> specification = new()
            {
                Conditions = new List<Expression<Func<AddressPath, bool>>>()
                {
                    a => a.Depth == 1
                }
            };

            List<AddressPath> addressPaths = await _repository.GetListAsync(dbContext, specification);

            return addressPaths.OrderBy(a => a.Name).ToList();
        }

        public async Task<List<AddressPath>> GetAllAddressPathsByParentIdAsync(int parentId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<AddressPath> specification = new()
            {
                Conditions = new List<Expression<Func<AddressPath, bool>>>()
                {
                    a => a.ParentPathId == parentId
                },
                Includes = a => a.Include(x => x.PreviousPath)
            };

            List<AddressPath> addressPaths = await _repository.GetListAsync(dbContext, specification);

            return addressPaths.OrderBy(a => a.Name).ToList();
        }

        public async Task<AddressPath> GetAddressPathByIdAsync(int id)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<AddressPath> specification = new()
            {
                Conditions = new List<Expression<Func<AddressPath, bool>>>()
                {
                    a => a.Id == id
                },
                Includes = a => a.Include(x => x.PreviousPath).ThenInclude(x => x.PreviousPath)
            };

            AddressPath addressPath = await _repository.GetAsync(dbContext, specification);

            return addressPath;
        }
    }
}