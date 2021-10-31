using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using VMS.Application.Interfaces;
using VMS.Application.ViewModels;
using VMS.Domain.Interfaces;
using VMS.Domain.Models;
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
    }
}