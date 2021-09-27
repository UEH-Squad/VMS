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
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class OrganizationService : BaseService,IOrganizationService
    {
        private readonly UserManager<User> _userManager;
        public OrganizationService(UserManager<User> userManager, 
                                   IRepository repository,
                                   IDbContextFactory<VmsDbContext> dbContextFactory,
                                   IMapper mapper,
                                   IIdentityService identityService) : base(repository, dbContextFactory, mapper)
        {
            _userManager = userManager;
        }

        private User FindUserById(string userId)
        {
            return Task.Run(() => _userManager.FindByIdAsync(userId)).Result;
        }
        private async Task<User> GetOrganizer(string orgId)
        {
            var user = FindUserById(orgId);
            bool orgRole = Task.Run(() => _userManager.IsInRoleAsync(user, "Organizer")).Result;
            if (orgRole == true) return user;
            else return null;
        }
        private User GetOrg(string orgId)
        {
            if (GetOrganizer(orgId) != null)
                return Task.Run(() => _userManager.Users.Include(x => x.UserAreas).ThenInclude(x => x.Area)
                                                   .Include(x => x.Activities)
                                                   .Include(x => x.Recruitments).ThenInclude(x => x.RecruitmentRatings)
                                                   .SingleOrDefaultAsync(x => x.Id == orgId)).Result;
            else return null;
        }
        public  UserViewModel GetOrgFull(string id)
        {
            User org = GetOrg(id);
            UserViewModel orgRatingViewModels = new();
            orgRatingViewModels = _mapper.Map<UserViewModel>(org);
            List<Recruitment> recruitments = org.Recruitments.ToList();
            double quantityRating = 0;
            double sumRating = 0;
            foreach(var rcm in recruitments)
            {
                var item = rcm.RecruitmentRatings.FirstOrDefault(r => r.IsOrgRating == false);
                if(item != null)
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
            else orgRatingViewModels.AverageRating = 5;

            return orgRatingViewModels; 
        }

        public async Task UpdateUserAsync(UpdateUserViewModel userViewModel, string userId)
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
            user.Avatar = userViewModel.Avatar;

            await _repository.UpdateAsync(dbContext, user);
        }
    }
}
