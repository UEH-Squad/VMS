using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly UserManager<User> _userManager;
        public OrganizationService(UserManager<User> userManager)
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
        public User GetOrg(string orgId)
        {
            if (GetOrganizer(orgId) != null)
                return Task.Run(() => _userManager.Users.Include(x => x.UserAreas).ThenInclude(x => x.Area)
                                                   .Include(x => x.Activities)
                                                   .Include(x => x.Recruitments).ThenInclude(x => x.RecruitmentRatings)
                                                   .SingleOrDefaultAsync(x => x.Id == orgId)).Result;
            else return null;
        }
        public  OrgRatingViewModel GetOrgRating(string Id)
        {
            User org = GetOrg(Id);
            List<Recruitment> recruitments = org.Recruitments.ToList();
            double QuantityRating = 0;
            double SumRating = 0;
            foreach(var rcm in recruitments)
            {
                var item = rcm.RecruitmentRatings.FirstOrDefault(r => r.IsOrgRating == false);
                if(item != null)
                {
                    QuantityRating = QuantityRating + 1;
                    SumRating = SumRating + item.Rank;
                }
            }
            OrgRatingViewModel orgRatingViewModels = new OrgRatingViewModel();
            orgRatingViewModels.FullName = org.FullName;
            orgRatingViewModels.Avatar = org.Avatar;
            orgRatingViewModels.CreatedDate = org.CreatedDate;
            orgRatingViewModels.Activities = org.Activities;
            orgRatingViewModels.Mission = org.Mission;
            orgRatingViewModels.UserAreas = org.UserAreas;
            orgRatingViewModels.Email = org.Email;
            orgRatingViewModels.PhoneNumber = org.PhoneNumber;
            orgRatingViewModels.QuantityRating = QuantityRating;
            if (QuantityRating != 0)
            {
                orgRatingViewModels.AverageRating = (float)Math.Round(SumRating / QuantityRating, 1);
            }
            else orgRatingViewModels.AverageRating = 5;

            return orgRatingViewModels; 
        }
    }
}
