using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class ListVolunteerService : BaseService, IListVolunteerService
    {
        public ListVolunteerService(IRepository repository,
                       IDbContextFactory<VmsDbContext> dbContextFactory,
                       IMapper mapper) : base(repository, dbContextFactory, mapper)
        {}

        public async Task<PaginatedList<ListVolunteerViewModel>> GetListVolunteersAsync(int actId, string searchValue, bool isDeleted, int currentPage)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<Recruitment> specification = new()
            {
                Conditions = new List<Expression<Func<Recruitment, bool>>>()
                {
                    a => a.ActivityId == actId,
                    a => a.User.FullName.ToUpper().Trim().Contains(searchValue.ToUpper().Trim()),
                    a => a.IsDeleted == isDeleted
                },
                Includes = a => a.Include(a => a.User).ThenInclude(a=> a.Faculty),
                PageIndex = currentPage,
                PageSize = 20
            };
            PaginatedList<Recruitment> recruitments = await _repository.GetListAsync(dbContext, specification);
            PaginatedList<ListVolunteerViewModel> paginatedList = new(
                recruitments.Items.Select(x =>  new ListVolunteerViewModel()
                    {
                    Id = x.Id,
                    Desire = x.Desire,
                    IsCommit = x.IsCommit,
                    UserId = x.UserId,
                    PhoneNumber = x.PhoneNumber,
                    User = x.User
                }).ToList(),
                recruitments.TotalItems,
                currentPage,
                recruitments.PageSize
                );

            return paginatedList;

        }

        public async Task UpdateVounteerAsync(int id, bool isDeleted)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Recruitment rec = await _repository.GetByIdAsync<Recruitment>(dbContext, id);
            rec.IsDeleted = isDeleted;
            await _repository.UpdateAsync(dbContext, rec);
        }
        
    }
}
