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
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
using VMS.GenericRepository;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class RecruitmentService : BaseService, IRecruitmentService
    {
        private IIdentityService _identityService;
        public RecruitmentService(IRepository repository, IDbContextFactory<VmsDbContext> dbContextFactory, IMapper mapper, IIdentityService identityService) : base(repository, dbContextFactory, mapper)
        {
            _identityService = identityService;
        }

        public async Task<PaginatedList<RecruitmentViewModel>> GetAllRecruitmentsAsync(int activityId, int currentPage)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<Recruitment> specification = new()
            {
                Conditions = new List<Expression<Func<Recruitment, bool>>>()
                {
                    r => r.ActivityId == activityId,
                    r => r.AcceptTime > r.EnrollTime
                },
                Includes = r => r.Include(x => x.RecruitmentRatings),
                PageIndex = currentPage,
                PageSize = 20,
            };

            PaginatedList<Recruitment> recruitments = await _repository.GetListAsync(dbContext, specification);

            PaginatedList<RecruitmentViewModel> paginatedList = new(
                recruitments.Items.Select(x => new RecruitmentViewModel()
                {
                    Id = x.Id,
                    Rating = x.RecruitmentRatings.FirstOrDefault(z => z.IsOrgRating)?.Rank,
                    User = _identityService.FindUserById(x.UserId)
                }).ToList(),
                recruitments.TotalItems,
                currentPage,
                recruitments.PageSize
            );
            return paginatedList;
        }

        public async Task UpdateRatingAsync(double rank, int? recruitmentId = null)
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
                RecruitmentRating recruitmentRating = recruitment.RecruitmentRatings.FirstOrDefault(x => x.IsOrgRating);
                if (recruitmentRating is null)
                {
                    recruitment.RecruitmentRatings.Add(new()
                    {
                        Rank = rank,
                        IsOrgRating = true
                    });
                }
                else
                {
                    recruitmentRating.Rank = rank;
                }
                await _repository.UpdateAsync(dbContext, recruitment);
            }

            //await _repository.UpdateAsync(dbContext, recruitments);
        }
    }
}
