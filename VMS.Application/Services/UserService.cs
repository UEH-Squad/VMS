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
    public class UserService : BaseService, IUserService
    {
        public UserService(IRepository repository,
                           IDbContextFactory<VmsDbContext> dbContextFactory,
                           IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
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
            });

            user = _mapper.Map(userProfileViewModel, user);

            user.UserAreas = MapAreas(userProfileViewModel, user);
            user.UserSkills = MapSkills(userProfileViewModel, user);
            user.UserAddresses = MapUserAddresses(userProfileViewModel, user);

            await _repository.UpdateAsync(dbContext, user);
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
    }
}