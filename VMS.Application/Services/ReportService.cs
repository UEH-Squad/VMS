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
    public class ReportService : BaseService, IReportService
    {
        public ReportService(IIdentityService identityService,
                                   IRepository repository,
                                   IDbContextFactory<VmsDbContext> dbContextFactory,
                                   IMapper mapper) : base(repository, dbContextFactory, mapper)
        {
        }
        public async Task AddReport(ReportViewModel reportViewModel)
        {
            DbContext dbContext = _dbContextFactory.CreateDbContext();

            Feedback report = _mapper.Map<Feedback>(reportViewModel);
            report.CreatedDate = DateTime.Now;
            report.CreatedBy = report.UserId;

            await _repository.InsertAsync(dbContext, report);
        }
    }
}
