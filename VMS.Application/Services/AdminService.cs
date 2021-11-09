using AutoMapper;
using Microsoft.AspNetCore.Identity;
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

        public async Task<bool> AddListUserAsync(List<CreateAccountViewModel> accounts, Role role)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            if (await IsExistAnyUserAsync(dbContext, accounts))
            {
                return false;
            }

            IEnumerable<User> users = _mapper.Map<List<User>>(accounts);

            PasswordHasher<User> hasher = new();
            foreach (var user in users)
            {
                user.PasswordHash = hasher.HashPassword(user, user.StudentId);
            }

            await _repository.InsertAsync(dbContext, users);

            await AddListUsersToRole(dbContext, users, role);

            return true;
        }

        private async Task<bool> IsExistAnyUserAsync(DbContext dbContext, List<CreateAccountViewModel> accounts)
        {
            Expression<Func<User, bool>> predicate = acc => accounts.Select(x => x.Email)
                                                                    .Any(x => x == acc.Email);

            return await _repository.ExistsAsync(dbContext, predicate);
        }

        private async Task AddListUsersToRole(DbContext dbContext, IEnumerable<User> users, Role userRole)
        {
            Specification<IdentityRole> specification = new()
            {
                Conditions = new List<Expression<Func<IdentityRole, bool>>>()
                {
                    x => x.Name == userRole.ToString()
                }
            };

            IdentityRole role = await _repository.GetAsync(dbContext, specification);

            IEnumerable<IdentityUserRole<string>> userRoles = users.Select(x => new IdentityUserRole<string>()
            {
                RoleId = role.Id,
                UserId = x.Id
            });

            await _repository.InsertAsync(dbContext, userRoles);
        }
    }
}
