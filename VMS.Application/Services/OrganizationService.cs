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
using VMS.Common;
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

        private User CheckRoleOrg(string orgId)
        {
            var user = Task.Run(() => _userManager.FindByIdAsync(orgId)).Result;
            bool orgRole = Task.Run(() => _userManager.IsInRoleAsync(user, Role.Organization.ToString())).Result;
            return (orgRole switch
            {
                true => user,
                _ => null
            });
        }
        public UserViewModel GetOrgFull(string id)
        {
            User org;
            if (CheckRoleOrg(id) != null)
            {
                org = Task.Run(() => _userManager.Users.Include(x => x.UserAreas)
                                                       .ThenInclude(x => x.Area)
                                                       .Include(x => x.Activities)
                                                       .Include(x => x.Recruitments)
                                                       .ThenInclude(x => x.RecruitmentRatings)
                                                       .SingleOrDefaultAsync(x => x.Id == id)).Result;
                UserViewModel orgRatingViewModels = new();
                orgRatingViewModels = _mapper.Map<UserViewModel>(org);
                List<Recruitment> recruitments = org.Recruitments.ToList();
                double quantityRating = 0;
                double sumRating = 0;
                foreach (var rcm in recruitments)
                {
                    var item = rcm.RecruitmentRatings.FirstOrDefault(r => r.IsOrgRating == false);
                    if (item != null)
                    {
                        quantityRating++;
                        sumRating += item.Rank;
                    }
                }
                orgRatingViewModels.QuantityRating = quantityRating;
                if (quantityRating != 0)
                {
                    orgRatingViewModels.AverageRating = (float)Math.Round(sumRating / quantityRating, 1);
                }
                else
                {
                    orgRatingViewModels.AverageRating = 5;
                }

                return orgRatingViewModels;
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
