using AutoMapper;
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
    public class ReportService : BaseService, IReportService
    {
        public ReportService(IRepository repository,
                             IDbContextFactory<VmsDbContext> dbContextFactory,
                             IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }

        public async Task AddReportAsync(ReportViewModel reportViewModel)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Feedback report = _mapper.Map<Feedback>(reportViewModel);

            report.ReasonReports = reportViewModel.Reasons.Select(x => new ReasonReport()
            {
                Reason = x,
                Feedback = report
            }).ToList();

            report.ImageReports = reportViewModel.Images.Select(x => new ImageReport()
            {
                Image = x,
                Feedback = report
            }).ToList();

            await _repository.InsertAsync(dbContext, report);
        }

        public async Task<PaginatedList<ReportViewModel>> GetAllReportsAsync(FilterReportViewModel filter, int currentPage)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            PaginationSpecification<Feedback> specification = new()
            {
                Conditions = new()
                {
                    x => !x.IsDeleted,
                    x => !filter.IsReportUser.HasValue || x.IsReportUser == filter.IsReportUser.Value,
                    GetConditionByReportState(filter.State),
                    x => !filter.Time.HasValue || (x.CreatedDate.Month == filter.Time.Value.Month
                                                  && x.CreatedDate.Year == filter.Time.Value.Year),
                },
                OrderBy = x => x.OrderByDescending(x => x.IsPinned)
                                .ThenByDescending(x => x.CreatedDate),
                Includes = x => x.Include(x => x.User)
                                 .Include(x => x.Activity),
                PageIndex = currentPage,
                PageSize = 20
            };

            PaginatedList<Feedback> feedbacks = await _repository.GetListAsync(dbContext, specification);

            return _mapper.Map<PaginatedList<ReportViewModel>>(feedbacks);
        }

        private static Expression<Func<Feedback, bool>> GetConditionByReportState(ReportState state)
        {
            return state switch
            {
                ReportState.Pinned => x => x.IsPinned,
                ReportState.Done => x => x.IsDone.HasValue && x.IsDone.Value,
                ReportState.Processing => x => x.IsDone.HasValue && !x.IsDone.Value,
                ReportState.Closed => x => x.IsClosed,
                ReportState.Deleted => x => true,
                _ => x => true
            };
        }

        public async Task UpdateReportStateAsync(int reportId, ReportState state)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Feedback feedback = await _repository.GetByIdAsync<Feedback>(dbContext, reportId);

            switch (state)
            {
                case ReportState.Pinned:
                    feedback.IsPinned = !feedback.IsPinned;
                    break;
                case ReportState.Done:
                    feedback.IsDone = true;
                    break;
                case ReportState.Processing:
                    feedback.IsDone = false;
                    break;
                case ReportState.Closed:
                    feedback.IsClosed = !feedback.IsClosed;
                    break;
                case ReportState.Deleted:
                    feedback.IsDeleted = true;
                    break;
            }

            await _repository.UpdateAsync(dbContext, feedback);
        }
    }
}