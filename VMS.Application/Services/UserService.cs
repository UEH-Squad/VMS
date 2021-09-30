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

        public async Task<CreateUserProfileViewModel> GetCreateUserProfileViewModelAsync(string userId)
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
                                 .Include(x => x.Faculty)
            });

            if (user is null)
            {
                return null;
            }

            CreateUserProfileViewModel userProfileViewModel = _mapper.Map<CreateUserProfileViewModel>(user);

            userProfileViewModel.Areas = user.UserAreas.Select(a => new AreaViewModel
            {
                Id = a.AreaId,
                Name = a.Area.Name,
                Icon = a.Area.Icon
            }).ToList();

            userProfileViewModel.Skills = user.UserSkills.Select(a => new SkillViewModel
            {
                Id = a.SkillId,
                Name = a.Skill.Name
            }).ToList();

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
            });

            user = _mapper.Map(userProfileViewModel, user);

            user.UserAreas = MapAreas(userProfileViewModel, user);
            user.UserSkills = MapSkills(userProfileViewModel, user);

            await _repository.UpdateAsync(dbContext, user);
        }

        private static ICollection<UserArea> MapAreas(CreateUserProfileViewModel userProfileViewModel, User user)
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
    }
}
