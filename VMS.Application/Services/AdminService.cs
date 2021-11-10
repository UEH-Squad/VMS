using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class AdminService : BaseService, IAdminService
    {
        public AdminService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<bool> AddListAccountsAsync(List<CreateAccountViewModel> accounts, Role role)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            if (await IsExistAnyUserAsync(dbContext, accounts))
            {
                return false;
            }

            IEnumerable<User> users = _mapper.Map<IEnumerable<User>>(accounts);

            AppRole userRole = await GetRoleAsync(dbContext, role);

            foreach (var user in users)
            {
                user.UserRoles = new List<UserRole>()
                {
                    new UserRole()
                    {
                        User = user,
                        Role = userRole
                    }
                };
            }

            await _repository.InsertAsync(dbContext, users);

            return true;
        }

        public async Task<bool> AddSingleAccountAsync(CreateAccountViewModel account, Role role)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            if (await IsExistAnyUserAsync(dbContext, account.Email))
            {
                return false;
            }

            User user = _mapper.Map<User>(account);

            AppRole userRole = await GetRoleAsync(dbContext, role);

            user.UserRoles = new List<UserRole>()
            {
                new UserRole()
                {
                    User = user,
                    Role = userRole
                }
            };

            await _repository.InsertAsync(dbContext, user);

            return true;
        }

        private async Task<bool> IsExistAnyUserAsync(DbContext dbContext, string email)
        {
            Expression<Func<User, bool>> predicate = acc => acc.Email == email || acc.UserName == email;

            return await _repository.ExistsAsync(dbContext, predicate);
        }

        private async Task<bool> IsExistAnyUserAsync(DbContext dbContext, List<CreateAccountViewModel> accounts)
        {
            Expression<Func<User, bool>> predicate = acc => accounts.Select(x => x.Email)
                                                                    .Any(x => x == acc.Email);

            return await _repository.ExistsAsync(dbContext, predicate);
        }

        private async Task<AppRole> GetRoleAsync(DbContext dbContext, Role role)
        {
            Specification<AppRole> specification = new()
            {
                Conditions = new List<Expression<Func<AppRole, bool>>>()
                {
                    x => x.Name == role.ToString()
                }
            };

            return await _repository.GetAsync(dbContext, specification);
        }
    }
}
