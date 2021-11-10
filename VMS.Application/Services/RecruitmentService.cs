﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class RecruitmentService : BaseService, IRecruitmentService
    {
        public RecruitmentService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task<PaginatedList<RecruitmentViewModel>> GetAllActivitiesAsync(FilterRecruitmentViewModel filter, string userId, int currentPage, string searchValue, bool? isRated)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<Recruitment> specification = new()
            {
                Conditions = new List<Expression<Func<Recruitment, bool>>>()
                {
                    r => r.UserId == userId,
                    GetActivitiesFromSearchValueAndFilter(filter, searchValue, isRated)
                },
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
                    Activity = x.Activity,
                    User = x.User,
                    Rating = x.RecruitmentRatings.FirstOrDefault(z => z.IsOrgRating && !z.IsReport)?.Rank,
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
                    GetConditionFromSearchValueAndFilter(searchValue, isRated)
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
                    User = x.User,
                    RecruitmentRatings = _mapper.Map<List<RecruitmentRatingViewModel>>(x.RecruitmentRatings)
                }).ToList(),
                recruitments.TotalItems,
                currentPage,
                recruitments.PageSize
            );

            return paginatedList;
        }

        public async Task UpdateRatingAndCommentAsync(double? rank, string comment, int? recruitmentId = null)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Specification<Recruitment> specification = new()
            {
                Includes = r => r.Include(x => x.RecruitmentRatings)
            };

            if (recruitmentId.HasValue)
            {
                specification.Conditions.Add(r => r.Id == recruitmentId.Value);
            }

            List<Recruitment> recruitments = await _repository.GetListAsync(dbContext, specification);

            foreach (var recruitment in recruitments)
            {
                RecruitmentRating recruitmentRating = recruitment.RecruitmentRatings.FirstOrDefault(x => x.IsOrgRating && !x.IsReport);

                if (recruitmentRating is null)
                {
                    recruitment.RecruitmentRatings.Add(new()
                    {
                        Rank = rank ?? 0,
                        Comment = comment,
                        IsOrgRating = true
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

        private static Expression<Func<Recruitment, bool>> GetConditionFromSearchValueAndFilter(string searchValue, bool? isRated)
        {
            if (isRated.HasValue)
            {
                if (isRated.Value)
                {
                    return r => r.RecruitmentRatings.Any(x => x.IsOrgRating && !x.IsReport && x.Rank != 0);
                }
                else
                {
                    return r => r.RecruitmentRatings.Any(x => x.IsOrgRating && !x.IsReport && x.Rank == 0) || !r.RecruitmentRatings.Any(x => x.IsOrgRating && !x.IsReport);
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                return r => r.User.FullName.ToLower().Contains(searchValue.ToLower());
            }

            return r => true;
        }

        private static Expression<Func<Recruitment, bool>> GetActivitiesFromSearchValueAndFilter(FilterRecruitmentViewModel filter, string searchValue, bool? isRated)
        {
            DateTime month12 = new DateTime(2021, 12, 31);
            DateTime month8 = new DateTime(2021, 8, 1);

            if (isRated.HasValue)
            {
                if (isRated.Value)
                {
                    return r => r.RecruitmentRatings.Any(x => x.IsOrgRating && !x.IsReport && x.Rank != 0);
                }
                else
                {
                    return r => r.RecruitmentRatings.Any(x => x.IsOrgRating && !x.IsReport && x.Rank == 0) || !r.RecruitmentRatings.Any(x => x.IsOrgRating && !x.IsReport);
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                return r => r.Activity.Name.ToUpper().Trim().Contains(searchValue.ToUpper().Trim());
            }

            if (!string.IsNullOrEmpty(filter.FullName))
            {
                return r => r.Activity.Organizer.FullName == filter.FullName || string.IsNullOrEmpty(filter.FullName);
            }

            if (!string.IsNullOrEmpty(filter.Semester))
            {
                if(filter.Semester == "Học kỳ đầu")
                {

                }

                if(filter.Semester == "Học kỳ giữa")
                {

                }

                if(filter.Semester == "Học kỳ cuối")
                {
                    return r => r.Activity.StartDate > month8 && r.Activity.StartDate < month12;
                }
            }

            return r => true;
        }
    }
}
