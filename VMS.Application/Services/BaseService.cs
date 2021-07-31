using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VMS.Domain.Interfaces;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class BaseService
    {
        protected readonly IRepository _repository;
        protected readonly IDbContextFactory<VmsDbContext> _dbContextFactory;
        protected readonly IMapper _mapper;

        public BaseService(IRepository repository,
                           IDbContextFactory<VmsDbContext> dbContextFactory,
                           IMapper mapper)
        {
            _repository = repository;
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
        }
    }
}