using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Common.Enums;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class AdminService : BaseService, IAdminService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper, RoleManager<IdentityRole> roleManager) : base(repository, dbContextFactory, mapper)
        {
            _roleManager = roleManager;
        }

        public async Task AddListUserAsync(List<CreateAccountViewModel> accounts, Role role)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            IEnumerable<User> users = _mapper.Map<List<User>>(accounts);

            PasswordHasher<User> hasher = new();
            foreach (var user in users)
            {
                user.PasswordHash = hasher.HashPassword(user, user.StudentId);
            }

            await _repository.InsertAsync(dbContext, users);

            await AddListUsersToRole(dbContext, users, role);
        }

        private async Task AddListUsersToRole(DbContext dbContext, IEnumerable<User> users, Role role)
        {
            string roleId = await _roleManager.GetRoleIdAsync(new IdentityRole(role.ToString()));

            var userRoles = users.Select(x => new IdentityUserRole<string>()
            {
                RoleId = roleId,
                UserId = x.Id
            });

            await _repository.InsertAsync(dbContext, userRoles);
        }
    }
}
