using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using VMS.Domain.Interfaces;
using VMS.Infrastructure.Data.Context;

namespace VMS.Application.Services
{
    public class BaseService
    {
        protected readonly IRepository _repository;
        protected readonly IDbContextFactory<VmsDbContext> _dbContextFactory;
        protected readonly IMapper _mapper;
        protected readonly GeometryFactory _geometryFactory;

        public BaseService(IRepository repository,
                           IDbContextFactory<VmsDbContext> dbContextFactory,
                           IMapper mapper)
        {
            _repository = repository;
            _dbContextFactory = dbContextFactory;
            _mapper = mapper;
            _geometryFactory = NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);
        }
    }
}