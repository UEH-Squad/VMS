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
    public class OrganizationService : BaseService, IOrganizationService
    {
        private readonly UserManager<User> _userManager;
        public OrganizationService(UserManager<User> userManager,
                                   IRepository repository,
                                   IDbContextFactory<VmsDbContext> dbContextFactory,
                                   IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
            _userManager = userManager;
        }

        private bool IsInRole(User org, Role role)
        {
            return Task.Run(() => _userManager.IsInRoleAsync(org, role.ToString())).Result;
        }

        private static void CalculateTotalAndRankRating(ICollection<Activity> activities, out int totalRating, out double totalRank)
        {
            var recruitmentRatings = activities.SelectMany(x => x.Recruitments)
                                                    .SelectMany(x => x.RecruitmentRatings)
                                                    .Where(x => !x.IsOrgRating && !x.IsReport);
            totalRating = recruitmentRatings.Count();
            totalRank = recruitmentRatings.Sum(x => x.Rank);
        }

        public UserViewModel GetOrgFull(string id)
        {
            User org = Task.Run(() => _userManager.Users.Include(x => x.UserAreas)
                                                       .ThenInclude(x => x.Area)
                                                       .Include(x => x.Activities)
                                                       .ThenInclude(x => x.Recruitments)
                                                       .ThenInclude(x => x.RecruitmentRatings)
                                                       .FirstOrDefault(x => x.Id == id)).Result;

            if (IsInRole(org, Role.Organization))
            {
                UserViewModel orgViewModel = _mapper.Map<UserViewModel>(org);

                CalculateTotalAndRankRating(org.Activities, out int totalRating, out double totalRank);
                orgViewModel.QuantityRating = totalRating;
                orgViewModel.AverageRating = totalRating > 0 ? Math.Round(totalRank / totalRating, 1) : 5;

                return orgViewModel;
            }
            else
            {
                return null;
            }
        }

        public async Task UpdateUserAsync(UpdateUserViewModel updateUserViewModel, string userId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<User> specification = new()
            {
                Conditions = new List<Expression<Func<User, bool>>>
                {
                    a => a.Id == userId
                },
                Includes = a => a.Include(x => x.Activities)
            };

            User user = await _repository.GetAsync(dbContext, specification);
            user.UpdatedBy = userId;
            user.UpdatedDate = DateTime.Now;
            user.Avatar = updateUserViewModel.Avatar;

            await _repository.UpdateAsync(dbContext, user);
        }
    }
}
