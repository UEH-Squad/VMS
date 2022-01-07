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
                MapUsernameAndEmail(user, role);
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

            if (await IsExistAnyUserAsync(dbContext, role == Role.Admin ? account.UserName : account.Email))
            {
                return false;
            }

            AppRole userRole = await GetRoleAsync(dbContext, role);

            User user = _mapper.Map<User>(account);
            user.UserRoles = new List<UserRole>()
            {
                new UserRole()
                {
                    User = user,
                    Role = userRole
                }
            };
            MapUsernameAndEmail(user, role);

            await _repository.InsertAsync(dbContext, user);

            return true;
        }

        private async Task<bool> IsExistAnyUserAsync(DbContext dbContext, string email)
        {
            string normalizedEmail = email.ToUpper();

            Expression<Func<User, bool>> predicate = acc => acc.NormalizedEmail == normalizedEmail
                                                         || acc.NormalizedUserName == normalizedEmail;

            return await _repository.ExistsAsync(dbContext, predicate);
        }

        private async Task<bool> IsExistAnyUserAsync(DbContext dbContext, List<CreateAccountViewModel> accounts)
        {
            Expression<Func<User, bool>> predicate = acc => accounts.Select(x => x.Email.ToUpper())
                                                                    .Any(x => x == acc.NormalizedEmail);

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

        public async Task<PaginatedList<AccountViewModel>> GetAllAccountsAsync(FilterAccountViewModel filter, int page, int pageSize = 20)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<User> specification = new()
            {
                Conditions = GetConditionsByFilter(filter),
                OrderBy = GetOrderByFilter(filter),
                PageSize = pageSize,
                PageIndex = page
            };

            PaginatedList<User> users = await _repository.GetListAsync(dbContext, specification);

            return _mapper.Map<PaginatedList<AccountViewModel>>(users);
        }

        public async Task<List<AccountViewModel>> GetAllAccountsByRoleAsync(Role role)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            string userRole = role.ToString();

            Specification<User> specification = new()
            {
                Conditions = new List<Expression<Func<User, bool>>>()
                {
                    u => u.UserRoles.Any(x => x.Role.Name == userRole)
                },
                Includes = u => u.Include(x => x.Faculty) 
            };

            List<User> users = await _repository.GetListAsync(dbContext, specification);

            return _mapper.Map<List<AccountViewModel>>(users);
        }

        private static List<Expression<Func<User, bool>>> GetConditionsByFilter(FilterAccountViewModel filter)
        {
            if (filter.IsSearch)
            {
                return new List<Expression<Func<User, bool>>>()
                {
                    x => x.NormalizedEmail.Contains(filter.SearchValue.Trim().ToUpper())
                        || x.NormalizedUserName.Contains(filter.SearchValue.Trim().ToUpper()),
                    x => x.UserRoles.Any(x => x.Role.Name == filter.Role)
                };
            }
            else
            {
                return new List<Expression<Func<User, bool>>>()
                {
                    x => x.Course == filter.Course || string.IsNullOrEmpty(filter.Course),
                    x => x.UserRoles.Any(x => x.Role.Name == filter.Role)
                };
            }
        }

        private static Func<IQueryable<User>, IOrderedQueryable<User>> GetOrderByFilter(FilterAccountViewModel filter)
        {
            return filter.IsNewest ? x => x.OrderByDescending(u => u.CreatedDate) : x => x.OrderBy(u => u.CreatedDate);
        }

        public async Task DeleteListAccountsAsync(List<string> listAccountIds)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<User> specification = new()
            {
                Conditions = new List<Expression<Func<User, bool>>>()
                {
                    acc => listAccountIds.Any(x => x == acc.Id)
                },
                Includes = x => x.Include(x => x.UserRoles)
            };

            IEnumerable<User> users = await _repository.GetListAsync(dbContext, specification);

            foreach (User user in users)
            {
                user.UserRoles.Clear();
                user.IsDeleted = true;
                user.StudentId = user.NormalizedEmail = user.Email = user.UserName = user.NormalizedUserName = null;
            }

            await _repository.UpdateAsync(dbContext, users);
        }

        public async Task<bool> UpdateAccountAsync(AccountViewModel account, Role role)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            User user = await _repository.GetByIdAsync<User>(dbContext, account.Id);

            if (role == Role.Admin
                ? account.UserName.ToUpper() != user.NormalizedUserName && await IsExistAnyUserAsync(dbContext, account.UserName)
                : account.Email.ToUpper() != user.NormalizedEmail && await IsExistAnyUserAsync(dbContext, account.Email))
            {
                return false;
            }

            _mapper.Map(account, user);

            MapUsernameAndEmail(user, role);

            await _repository.UpdateAsync(dbContext, user);

            return true;
        }

        private static void MapUsernameAndEmail(User user, Role role)
        {
            if (role == Role.Admin)
            {
                user.Email = user.UserName;
                user.NormalizedEmail = user.NormalizedUserName;
            }
            else
            {
                user.UserName = user.Email;
                user.NormalizedUserName = user.NormalizedEmail;
            }
        }
    }
}
