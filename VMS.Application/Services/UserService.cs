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
using VMS.Common.Extensions;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IIdentityService _identityService;
        public UserService(IRepository repository,
                           IDbContextFactory<VmsDbContext> dbContextFactory,
                           IMapper mapper,
                           UserManager<User> userManager,
                           IIdentityService identityService) : base(repository, dbContextFactory, mapper)
        {
            _userManager = userManager;
            _identityService = identityService;
        }

        public async Task<CreateOrgProfileViewModel> GetOrgProfileViewModelAsync(string userId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            User user = await _repository.GetAsync(dbContext, new Specification<User>()
            {
                Conditions = new List<Expression<Func<User, bool>>>
                {
                    a => a.Id == userId
                },
                Includes = a => a.Include(x => x.UserAreas).ThenInclude(s => s.Area)
            });

            if (user is null)
            {
                return null;
            }

            return _mapper.Map<CreateOrgProfileViewModel>(user);
        }

        public async Task UpdateOrgProfile(CreateOrgProfileViewModel orgProfileViewModel, string userId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            User user = await _repository.GetAsync(dbContext, new Specification<User>()
            {
                Conditions = new List<Expression<Func<User, bool>>>
                {
                    a => a.Id == userId
                },
                Includes = a => a.Include(x => x.UserAreas).ThenInclude(s => s.Area)
            });

            user = _mapper.Map(orgProfileViewModel, user);

            user.UserAreas = MapAreas(orgProfileViewModel, user);

            await _repository.UpdateAsync(dbContext, user);
        }

        public async Task<CreateUserProfileViewModel> GetUserProfileViewModelAsync(string userId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            User user = await _repository.GetAsync(dbContext, new Specification<User>()
            {
                Conditions = new List<Expression<Func<User, bool>>>
                {
                    a => a.Id == userId
                },
                Includes = a => a.Include(x => x.UserAreas).ThenInclude(s => s.Area)
                                 .Include(x => x.UserSkills).ThenInclude(s => s.Skill)
                                 .Include(x => x.UserAddresses).ThenInclude(s => s.AddressPath)
                                 .Include(x => x.Faculty)
            });

            if (user is null)
            {
                return null;
            }

            CreateUserProfileViewModel userProfileViewModel = _mapper.Map<CreateUserProfileViewModel>(user);

            if (userProfileViewModel.FacultyId is not null)
            {
                userProfileViewModel.FacultyName = user.Faculty.Name;
            }

            UserAddress province = user.UserAddresses.FirstOrDefault(x => x.AddressPath.Depth == 1);
            if (province is not null)
            {
                userProfileViewModel.ProvinceId = province.AddressPath.Id;
                userProfileViewModel.Province = province.AddressPath.Name;
            }

            UserAddress district = user.UserAddresses.FirstOrDefault(x => x.AddressPath.Depth == 2);
            if (district is not null)
            {
                userProfileViewModel.DistrictId = district.AddressPath.Id;
                userProfileViewModel.District = district.AddressPath.Name;
            }

            UserAddress ward = user.UserAddresses.FirstOrDefault(x => x.AddressPath.Depth == 3);
            if (ward is not null)
            {
                userProfileViewModel.WardId = ward.AddressPath.Id;
                userProfileViewModel.Ward = ward.AddressPath.Name;
            }

            return userProfileViewModel;
        }

        public async Task UpdateUserProfile(CreateUserProfileViewModel userProfileViewModel, string userId)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            User user = await _repository.GetAsync(dbContext, new Specification<User>()
            {
                Conditions = new List<Expression<Func<User, bool>>>
                {
                    a => a.Id == userId
                },
                Includes = a => a.Include(x => x.UserAreas).ThenInclude(s => s.Area)
                                 .Include(x => x.UserSkills).ThenInclude(s => s.Skill)
                                 .Include(x => x.UserAddresses).ThenInclude(s => s.AddressPath)
                                 .Include(x => x.Faculty)
            });

            user = _mapper.Map(userProfileViewModel, user);

            user.UserAreas = MapAreas(userProfileViewModel, user);
            user.UserSkills = MapSkills(userProfileViewModel, user);
            user.UserAddresses = MapUserAddresses(userProfileViewModel, user);

            await _repository.UpdateAsync(dbContext, user);
        }
    
        private static ICollection<UserArea> MapAreas(UserProfileViewModel userProfileViewModel, User user)
        {
            return userProfileViewModel.Areas.Select(s => new UserArea
            {
                User = user,
                AreaId = s.Id
            }).ToList();
        }

        private static ICollection<UserSkill> MapSkills(CreateUserProfileViewModel userProfileViewModel, User user)
        {
            return userProfileViewModel.Skills.Select(s => new UserSkill
            {
                User = user,
                SkillId = s.Id
            }).ToList();
        }

        private static ICollection<UserAddress> MapUserAddresses(CreateUserProfileViewModel userProfileViewModel, User user)
        {
            List<UserAddress> result = new()
            {
                new() { User = user, AddressPathId = userProfileViewModel.ProvinceId },
                new() { User = user, AddressPathId = userProfileViewModel.DistrictId }
            };

            if (userProfileViewModel.WardId > 0)
            {
                result.Add(new() { User = user, AddressPathId = userProfileViewModel.WardId });
            }

            return result;
        }

        private User FindUserById(string userId)
        {
            return Task.Run(() => _userManager.Users.Include(x => x.UserAreas)
                                                    .ThenInclude(x => x.Area)
                                                    .Include(x => x.UserSkills)
                                                    .ThenInclude(x => x.Skill)
                                                    .Include(x => x.Recruitments)
                                                    .ThenInclude(x => x.RecruitmentRatings)
                                                    .FirstOrDefault(x => x.Id == userId)).Result;
        }

        private static void CalculateTotalAndRankRating(ICollection<Recruitment> recruitments, out int totalRating, out double totalRank)
        {
            var recruitmentRatings = recruitments.SelectMany(x => x.RecruitmentRatings)
                                                    .Where(x => x.IsOrgRating);
            totalRating = recruitmentRatings.Count();
            totalRank = recruitmentRatings.Sum(x => x.Rank);
        }

        private bool IsInRole(User user, Role role)
        {
            return Task.Run(() => _userManager.IsInRoleAsync(user, role.ToString())).Result;
        }

        public UserViewModel GetUserViewModel(string userId)
        {
            User user = FindUserById(userId);

            if (IsInRole(user, Role.User))
            {
                UserViewModel userViewModel = _mapper.Map<UserViewModel>(user);

                CalculateTotalAndRankRating(user.Recruitments, out int totalRating, out double totalRank);
                userViewModel.QuantityRating = totalRating;
                userViewModel.AverageRating = totalRating > 0 ? Math.Round(totalRank / totalRating, 1) : 5;

                return userViewModel;
            }
            else
            {
                return null;
            }
        }

        public void UpdateUserAvatar(string userId, string avatar)
        {
            User user = FindUserById(userId);

            user.Avatar = avatar;

            Task.Run(() => _userManager.UpdateAsync(user));
        }

        public async Task<HashSet<DateTime>> GetActivityDaysAsync(string userId, DateTime startDate, DateTime endDate)
        {
            User result = _identityService.GetUserWithFavoritesAndRecruitmentsById(userId);

            HashSet<DateTime> acts = result.Recruitments.Where(x => x.Activity.StartDate.Between(startDate, endDate)
                                                                    || x.Activity.EndDate.Between(startDate, endDate))
                                                        .Select(x => x.Activity.StartDate.GetDateRange(x.Activity.EndDate))
                                                        .SelectMany(x => x)
                                                        .ToHashSet();

            return await Task.FromResult(acts);
        }

        public List<UserViewModel> GetAllVolunteers()
        {
            var volunteers = Task.Run(() => _userManager.GetUsersInRoleAsync(Role.User.ToString())).Result;

            return _mapper.Map<List<UserViewModel>>(volunteers);
        }

        public async Task<PaginatedList<UserViewModel>> GetAllVolunteers(FilterVolunteerViewModel filter, int currentPage)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<User> specification = new()
            {
                Conditions = GetConditionsByFilter(filter),
                Includes = x => x.Include(x => x.Activities)
                                 .ThenInclude(x => x.Recruitments)
                                 .ThenInclude(x => x.RecruitmentRatings),
                PageIndex = currentPage,
                PageSize = 8,
            };

            PaginatedList<User> volunteers = await _repository.GetListAsync(dbContext, specification);

            PaginatedList<UserViewModel> paginatedList = _mapper.Map<PaginatedList<UserViewModel>>(volunteers);

            foreach (var volunteer in paginatedList.Items)
            {
                CalculateTotalAndRankRating(volunteer.Activities, out int totalRating, out double totalRank);
                volunteer.QuantityRating = totalRating;
                volunteer.AverageRating = totalRating > 0 ? Math.Round(totalRank / totalRating, 1) : 5;
            }

            return paginatedList;
        }

        private static List<Expression<Func<User, bool>>> GetConditionsByFilter(FilterVolunteerViewModel filter)
        {
            string vltRole = Role.User.ToString();

            if (filter.IsSearch)
            {
                return new List<Expression<Func<User, bool>>>()
                {
                    x => x.UserRoles.Any(x => x.Role.Name == vltRole),
                    x => x.FullName.ToLower().Contains(filter.SearchValue.ToLower())
                };
            }
            else
            {
                return new List<Expression<Func<User, bool>>>()
                {
                    x => x.UserRoles.Any(x => x.Role.Name == vltRole),
                    x => x.Course == filter.Course || string.IsNullOrEmpty(filter.Course),
                    a => a.Faculty.Name == filter.FacultyName || string.IsNullOrEmpty(filter.FacultyName),
                    a => a.UserAreas.Select(r => r.AreaId)
                                         .Where(userAreaId => filter.Areas.Select(area => area.Id)
                                                                           .Any(areaId => areaId == userAreaId))
                                         .Count() == filter.Areas.Count,
                    a => a.UserSkills.Select(r => r.SkillId)
                                         .Where(userSkillId => filter.Skills.Select(skill => skill.Id)
                                                                           .Any(skillId => skillId == userSkillId))
                                         .Count() == filter.Skills.Count
                };
            }
        }

        private static void CalculateTotalAndRankRating(ICollection<Activity> activities, out int totalRating, out double totalRank)
        {
            var recruitmentRatings = activities.SelectMany(x => x.Recruitments)
                                                    .SelectMany(x => x.RecruitmentRatings)
                                                    .Where(x => !x.IsOrgRating && !x.IsReport);
            totalRating = recruitmentRatings.Count();
            totalRank = recruitmentRatings.Sum(x => x.Rank);
        }
    }
}