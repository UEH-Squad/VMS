using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
    public class RecruitmentService : BaseService, IRecruitmentService
    {
        public RecruitmentService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<PaginatedList<RecruitmentViewModel>> GetAllActivitiesAsync(FilterRecruitmentViewModel filter, string userId, int currentPage)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<Recruitment> specification = new()
            {
                Conditions = GetActivityLogConditions(filter, userId),
                Includes = r => r.Include(x => x.User)
                                 .Include(x => x.RecruitmentRatings)
                                 .Include(x => x.Activity).ThenInclude(x => x.Organizer),
                PageIndex = currentPage,
                PageSize = 8,
            };

            PaginatedList<Recruitment> recruitments = await _repository.GetListAsync(dbContext, specification);

            PaginatedList<RecruitmentViewModel> paginatedList = new(
                recruitments.Items.Select(x => new RecruitmentViewModel()
                {
                    Id = x.Id,
                    Activity = _mapper.Map<ActivityViewModel>(x.Activity),
                    User = _mapper.Map<UserViewModel>(x.Activity.Organizer),
                    Rating = x.RecruitmentRatings.FirstOrDefault(z => !z.IsOrgRating && !z.IsReport)?.Rank,
                    RecruitmentRatings = _mapper.Map<List<RecruitmentRatingViewModel>>(x.RecruitmentRatings)
                }).ToList(),
                recruitments.TotalItems,
                currentPage,
                recruitments.PageSize
            );

            return paginatedList;
        }

        public async Task<PaginatedList<RecruitmentViewModel>> GetAllRecruitmentsAsync(int activityId, int currentPage, string searchValue, bool? isRated)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<Recruitment> specification = new()
            {
                Conditions = new List<Expression<Func<Recruitment, bool>>>()
                {
                    r => r.ActivityId == activityId,
                    GetConditionBySearchOrOrder(searchValue, isRated)
                },
                Includes = r => r.Include(x => x.User).Include(x => x.RecruitmentRatings),
                PageIndex = currentPage,
                PageSize = 20,
            };

            PaginatedList<Recruitment> recruitments = await _repository.GetListAsync(dbContext, specification);

            PaginatedList<RecruitmentViewModel> paginatedList = new(
                recruitments.Items.Select(x => new RecruitmentViewModel()
                {
                    Id = x.Id,
                    Rating = x.RecruitmentRatings.FirstOrDefault(z => z.IsOrgRating && !z.IsReport)?.Rank,
                    User = _mapper.Map<UserViewModel>(x.User),
                    RecruitmentRatings = _mapper.Map<List<RecruitmentRatingViewModel>>(x.RecruitmentRatings)
                }).ToList(),
                recruitments.TotalItems,
                currentPage,
                recruitments.PageSize
            );

            return paginatedList;
        }

        public async Task UpdateRatingAndCommentAsync(int activityId, double? rank, string comment, int? recruitmentId = null, bool isOrgRating = true)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Recruitment> specification = new()
            {
                Conditions = new List<Expression<Func<Recruitment, bool>>>()
                {
                    x => x.ActivityId == activityId
                },
                Includes = r => r.Include(x => x.RecruitmentRatings)
            };

            if (recruitmentId.HasValue)
            {
                specification.Conditions.Add(r => r.Id == recruitmentId.Value);
            }

            List<Recruitment> recruitments = await _repository.GetListAsync(dbContext, specification);

            foreach (var recruitment in recruitments)
            {
                RecruitmentRating recruitmentRating = recruitment.RecruitmentRatings.FirstOrDefault(x => x.IsOrgRating == isOrgRating && !x.IsReport);

                if (recruitmentRating is null)
                {
                    recruitment.RecruitmentRatings.Add(new()
                    {
                        Rank = rank ?? 0,
                        Comment = comment,
                        IsOrgRating = isOrgRating
                    });
                }
                else
                {
                    recruitmentRating.Rank = rank ?? recruitmentRating.Rank;
                    recruitmentRating.Comment = (rank.HasValue ? recruitmentRating.Comment : comment);
                }
            }

            await _repository.UpdateAsync<Recruitment>(dbContext, recruitments);
        }

        private static Expression<Func<Recruitment, bool>> GetConditionBySearchOrOrder(string searchValue, bool? isRated, bool isOrgRating = true)
        {
            if (isRated.HasValue)
            {
                if (isRated.Value)
                {
                    return r => r.RecruitmentRatings.Any(x => x.IsOrgRating == isOrgRating && !x.IsReport && x.Rank != 0);
                }
                else
                {
                    return r => r.RecruitmentRatings.Any(x => x.IsOrgRating == isOrgRating && !x.IsReport && x.Rank == 0)
                                || !r.RecruitmentRatings.Any(x => x.IsOrgRating == isOrgRating && !x.IsReport);
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                return r => isOrgRating ? r.User.FullName.ToLower().Contains(searchValue.ToLower())
                                        : r.Activity.Name.ToLower().Contains(searchValue.ToLower())
                                       || r.Activity.Organizer.FullName.ToLower().Contains(searchValue.ToLower());
            }

            return r => true;
        }

        private static Expression<Func<Recruitment, bool>> GetConditionBySemester(Semester semester)
        {
            switch (semester)
            {
                case Semester.First:
                    return r => r.EnrollTime.Month >= 1 && r.EnrollTime.Month <= 5;

                case Semester.Middle:
                    return r => r.EnrollTime.Month >= 6 && r.EnrollTime.Month <= 7;

                case Semester.Last:
                    return r => r.EnrollTime.Month >= 8 && r.EnrollTime.Month <= 12;

                default:
                    return r => true;
            }
        }

        private static List<Expression<Func<Recruitment, bool>>> GetActivityLogConditions(FilterRecruitmentViewModel filter, string userId)
        {
            if (filter.IsSearch)
            {
                return new List<Expression<Func<Recruitment, bool>>>()
                {
                    r => r.UserId == userId,
                    GetConditionBySearchOrOrder(filter.SearchValue, null, false)
                };
            }
            else
            {
                return new List<Expression<Func<Recruitment, bool>>>()
                {
                    r => r.UserId == userId,
                    r => r.Activity.OrgId == filter.OrgId || string.IsNullOrEmpty(filter.OrgId),
                    GetConditionBySemester(filter.Semester),
                    GetConditionBySearchOrOrder(string.Empty, filter.IsRated, false)
                };
            }
        }
    }
}
