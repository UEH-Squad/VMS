using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VMS.Common.Extensions;
using VMS.Domain.Enums;
using VMS.Domain.Interfaces;
using VMS.GenericRepository;

namespace VMS.Infrastructure.Data.Repositories
{
    public class Repository : IRepository
    {
        private readonly Func<CacheTech, ICacheService> _cacheService;
        private readonly static CacheTech cacheTech = CacheTech.Memory;

        public Repository(Func<CacheTech, ICacheService> cacheService)
        {
            _cacheService = cacheService;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync(
            DbContext dbContext,
            IsolationLevel isolationLevel = IsolationLevel.Unspecified,
            CancellationToken cancellationToken = default)
        {
            IDbContextTransaction dbContextTransaction = await dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return dbContextTransaction;
        }

        public IQueryable<T> GetQueryable<T>(DbContext dbContext)
            where T : class
        {
            return dbContext.Set<T>();
        }

        public async Task<List<T>> GetListAsync<T>(DbContext dbContext, CancellationToken cancellationToken = default)
            where T : class
        {
            return await GetListAsync<T>(dbContext, false, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(DbContext dbContext, bool asNoTracking, CancellationToken cancellationToken = default)
            where T : class
        {
            Func<IQueryable<T>, IIncludableQueryable<T, object>> nullValue = null;
            return await GetListAsync(dbContext, nullValue, asNoTracking, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(
            DbContext dbContext,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            CancellationToken cancellationToken = default)
            where T : class
        {
            return await GetListAsync(dbContext, includes, false, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(
            DbContext dbContext,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default)
            where T : class
        {
            string cacheKey = $"{nameof(GetListAsync)}-{typeof(T)}-{includes}-{asNoTracking}";
            if (_cacheService(cacheTech).TryGet(cacheKey, out List<T> entities))
            {
                return entities;
            }

            IQueryable<T> query = dbContext.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            entities = await query.ToListAsync(cancellationToken);

            _cacheService(cacheTech).Set(cacheKey, entities);

            return entities;
        }

        public async Task<List<T>> GetListAsync<T>(DbContext dbContext, Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
             where T : class
        {
            return await GetListAsync(dbContext, condition, false, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(
            DbContext dbContext,
            Expression<Func<T, bool>> condition,
            bool asNoTracking,
            CancellationToken cancellationToken = default)
             where T : class
        {
            return await GetListAsync(dbContext, condition, null, asNoTracking, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(
            DbContext dbContext,
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default)
             where T : class
        {
            string cacheKey = $"{nameof(GetListAsync)}-{typeof(T)}-{condition}-{includes}-{asNoTracking}";
            if (_cacheService(cacheTech).TryGet(cacheKey, out List<T> entities))
            {
                return entities;
            }

            IQueryable<T> query = dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            entities = await query.ToListAsync(cancellationToken);

            _cacheService(cacheTech).Set(cacheKey, entities);

            return entities;
        }

        public async Task<List<T>> GetListAsync<T>(DbContext dbContext, Specification<T> specification, CancellationToken cancellationToken = default)
           where T : class
        {
            return await GetListAsync(dbContext, specification, false, cancellationToken);
        }

        public async Task<List<T>> GetListAsync<T>(DbContext dbContext, Specification<T> specification, bool asNoTracking, CancellationToken cancellationToken = default)
           where T : class
        {
            string cacheKey = $"{nameof(GetListAsync)}-{typeof(T)}-{specification}-{asNoTracking}";
            if (_cacheService(cacheTech).TryGet(cacheKey, out List<T> entities))
            {
                return entities;
            }

            IQueryable<T> query = dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            entities = await query.ToListAsync(cancellationToken);

            _cacheService(cacheTech).Set(cacheKey, entities);

            return entities;
        }

        public async Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(
            DbContext dbContext,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            string cacheKey = $"{nameof(GetListAsync)}-{typeof(T)}-{selectExpression}";
            if (_cacheService(cacheTech).TryGet(cacheKey, out List<TProjectedType> entities))
            {
                return entities;
            }

            entities = await dbContext.Set<T>().Select(selectExpression).ToListAsync(cancellationToken);

            _cacheService(cacheTech).Set(cacheKey, entities);

            return entities;
        }

        public async Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(
            DbContext dbContext,
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            string cacheKey = $"{nameof(GetListAsync)}-{typeof(T)}-{condition}-{selectExpression}";
            if (_cacheService(cacheTech).TryGet(cacheKey, out List<TProjectedType> entities))
            {
                return entities;
            }

            IQueryable<T> query = dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            entities = await query.Select(selectExpression).ToListAsync(cancellationToken);

            _cacheService(cacheTech).Set(cacheKey, entities);

            return entities;
        }

        public async Task<List<TProjectedType>> GetListAsync<T, TProjectedType>(
            DbContext dbContext,
            Specification<T> specification,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            string cacheKey = $"{nameof(GetListAsync)}-{typeof(T)}-{specification}-{selectExpression}";
            if (_cacheService(cacheTech).TryGet(cacheKey, out List<TProjectedType> entities))
            {
                return entities;
            }

            IQueryable<T> query = dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            entities = await query.Select(selectExpression).ToListAsync(cancellationToken);

            _cacheService(cacheTech).Set(cacheKey, entities);

            return await query.Select(selectExpression).ToListAsync(cancellationToken);
        }

        public async Task<PaginatedList<T>> GetListAsync<T>(
            DbContext dbContext,
            PaginationSpecification<T> specification,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            string cacheKey = $"{nameof(GetListAsync)}-{typeof(T)}-{specification}";
            if (_cacheService(cacheTech).TryGet(cacheKey, out PaginatedList<T> paginatedList))
            {
                return paginatedList;
            }

            paginatedList = await dbContext.Set<T>().ToPaginatedListAsync(specification, cancellationToken);

            _cacheService(cacheTech).Set(cacheKey, paginatedList);

            return paginatedList;
        }

        public async Task<PaginatedList<TProjectedType>> GetListAsync<T, TProjectedType>(
            DbContext dbContext,
            PaginationSpecification<T> specification,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
            where TProjectedType : class
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            string cacheKey = $"{nameof(GetListAsync)}-{typeof(T)}-{specification}-{selectExpression}";
            if (_cacheService(cacheTech).TryGet(cacheKey, out PaginatedList<TProjectedType> paginatedList))
            {
                return paginatedList;
            }

            IQueryable<T> query = dbContext.Set<T>().GetSpecifiedQuery((SpecificationBase<T>)specification);

            paginatedList = await query.Select(selectExpression)
                .ToPaginatedListAsync(specification.PageIndex, specification.PageSize, cancellationToken);

            _cacheService(cacheTech).Set(cacheKey, paginatedList);

            return paginatedList;
        }

        public async Task<T> GetByIdAsync<T>(DbContext dbContext, object id, CancellationToken cancellationToken = default)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            T enity = await GetByIdAsync<T>(dbContext, id, false, cancellationToken);
            return enity;
        }

        public async Task<T> GetByIdAsync<T>(DbContext dbContext, object id, bool asNoTracking, CancellationToken cancellationToken = default)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            T enity = await GetByIdAsync<T>(dbContext, id, null, asNoTracking, cancellationToken);
            return enity;
        }

        public async Task<T> GetByIdAsync<T>(
            DbContext dbContext,
            object id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            T enity = await GetByIdAsync(dbContext, id, includes, false, cancellationToken);
            return enity;
        }

        public async Task<T> GetByIdAsync<T>(
            DbContext dbContext,
            object id,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking = false,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            IEntityType entityType = dbContext.Model.FindEntityType(typeof(T));

            string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

            IQueryable<T> query = dbContext.Set<T>();

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            T enity = await query.FirstOrDefaultAsync(expressionTree, cancellationToken);
            return enity;
        }

        public async Task<TProjectedType> GetByIdAsync<T, TProjectedType>(
            DbContext dbContext,
            object id,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IEntityType entityType = dbContext.Model.FindEntityType(typeof(T));

            string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();
            Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

            if (primaryKeyName == null || primaryKeyType == null)
            {
                throw new ArgumentException("Entity does not have any primary key defined", nameof(id));
            }

            object primayKeyValue = null;

            try
            {
                primayKeyValue = Convert.ChangeType(id, primaryKeyType, CultureInfo.InvariantCulture);
            }
            catch (Exception)
            {
                throw new ArgumentException($"You can not assign a value of type {id.GetType()} to a property of type {primaryKeyType}");
            }

            ParameterExpression pe = Expression.Parameter(typeof(T), "entity");
            MemberExpression me = Expression.Property(pe, primaryKeyName);
            ConstantExpression constant = Expression.Constant(primayKeyValue, primaryKeyType);
            BinaryExpression body = Expression.Equal(me, constant);
            Expression<Func<T, bool>> expressionTree = Expression.Lambda<Func<T, bool>>(body, new[] { pe });

            IQueryable<T> query = dbContext.Set<T>();

            return await query.Where(expressionTree).Select(selectExpression).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> GetAsync<T>(
            DbContext dbContext,
            Expression<Func<T, bool>> condition,
            CancellationToken cancellationToken = default)
           where T : class
        {
            return await GetAsync(dbContext, condition, null, false, cancellationToken);
        }

        public async Task<T> GetAsync<T>(
            DbContext dbContext,
            Expression<Func<T, bool>> condition,
            bool asNoTracking,
            CancellationToken cancellationToken = default)
           where T : class
        {
            return await GetAsync(dbContext, condition, null, asNoTracking, cancellationToken);
        }

        public async Task<T> GetAsync<T>(
            DbContext dbContext,
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            CancellationToken cancellationToken = default)
           where T : class
        {
            return await GetAsync(dbContext, condition, includes, false, cancellationToken);
        }

        public async Task<T> GetAsync<T>(
            DbContext dbContext,
            Expression<Func<T, bool>> condition,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> includes,
            bool asNoTracking,
            CancellationToken cancellationToken = default)
           where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            if (includes != null)
            {
                query = includes(query);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<T> GetAsync<T>(DbContext dbContext, Specification<T> specification, CancellationToken cancellationToken = default)
            where T : class
        {
            return await GetAsync(dbContext, specification, false, cancellationToken);
        }

        public async Task<T> GetAsync<T>(DbContext dbContext, Specification<T> specification, bool asNoTracking, CancellationToken cancellationToken = default)
            where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }

            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TProjectedType> GetAsync<T, TProjectedType>(
            DbContext dbContext,
            Expression<Func<T, bool>> condition,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<TProjectedType> GetAsync<T, TProjectedType>(
            DbContext dbContext,
            Specification<T> specification,
            Expression<Func<T, TProjectedType>> selectExpression,
            CancellationToken cancellationToken = default)
            where T : class
        {
            if (selectExpression == null)
            {
                throw new ArgumentNullException(nameof(selectExpression));
            }

            IQueryable<T> query = dbContext.Set<T>();

            if (specification != null)
            {
                query = query.GetSpecifiedQuery(specification);
            }

            return await query.Select(selectExpression).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> ExistsAsync<T>(DbContext dbContext, CancellationToken cancellationToken = default)
           where T : class
        {
            return await ExistsAsync<T>(dbContext, null, cancellationToken);
        }

        public async Task<bool> ExistsAsync<T>(DbContext dbContext, Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
           where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (condition == null)
            {
                return await query.AnyAsync(cancellationToken);
            }

            bool isExists = await query.AnyAsync(condition, cancellationToken);
            return isExists;
        }

        public async Task<object[]> InsertAsync<T>(DbContext dbContext, T entity, CancellationToken cancellationToken = default)
           where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityEntry<T> entityEntry = await dbContext.Set<T>().AddAsync(entity, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            object[] primaryKeyValue = entityEntry.Metadata.FindPrimaryKey().Properties.
                Select(p => entityEntry.Property(p.Name).CurrentValue).ToArray();

            BackgroundJob.Enqueue(() => RefreshCache());

            return primaryKeyValue;
        }

        public async Task InsertAsync<T>(DbContext dbContext, IEnumerable<T> entities, CancellationToken cancellationToken = default)
           where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            await dbContext.Set<T>().AddRangeAsync(entities, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task UpdateAsync<T>(DbContext dbContext, T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            EntityEntry<T> trackedEntity = dbContext.ChangeTracker.Entries<T>().FirstOrDefault(x => x.Entity == entity);

            if (trackedEntity == null)
            {
                IEntityType entityType = dbContext.Model.FindEntityType(typeof(T));

                if (entityType == null)
                {
                    throw new InvalidOperationException($"{typeof(T).Name} is not part of EF Core DbContext model");
                }

                string primaryKeyName = entityType.FindPrimaryKey().Properties.Select(p => p.Name).FirstOrDefault();

                if (primaryKeyName != null)
                {
                    Type primaryKeyType = entityType.FindPrimaryKey().Properties.Select(p => p.ClrType).FirstOrDefault();

                    object primaryKeyDefaultValue = primaryKeyType.IsValueType ? Activator.CreateInstance(primaryKeyType) : null;

                    object primaryValue = entity.GetType().GetProperty(primaryKeyName).GetValue(entity, null);

                    if (primaryKeyDefaultValue.Equals(primaryValue))
                    {
                        throw new InvalidOperationException("The primary key value of the entity to be updated is not valid.");
                    }
                }

                dbContext.Set<T>().Update(entity);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task UpdateAsync<T>(DbContext dbContext, IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            dbContext.Set<T>().UpdateRange(entities);
            await dbContext.SaveChangesAsync(cancellationToken);

            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task DeleteAsync<T>(DbContext dbContext, T entity, CancellationToken cancellationToken = default)
            where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            dbContext.Set<T>().Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task DeleteAsync<T>(DbContext dbContext, IEnumerable<T> entities, CancellationToken cancellationToken = default)
            where T : class
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            dbContext.Set<T>().RemoveRange(entities);
            await dbContext.SaveChangesAsync(cancellationToken);

            BackgroundJob.Enqueue(() => RefreshCache());
        }

        public async Task<int> GetCountAsync<T>(DbContext dbContext, CancellationToken cancellationToken = default)
            where T : class
        {
            int count = await dbContext.Set<T>().CountAsync(cancellationToken);
            return count;
        }

        public async Task<int> GetCountAsync<T>(DbContext dbContext, Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
            where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<int> GetCountAsync<T>(DbContext dbContext, IEnumerable<Expression<Func<T, bool>>> conditions, CancellationToken cancellationToken = default)
            where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (conditions != null)
            {
                foreach (Expression<Func<T, bool>> expression in conditions)
                {
                    query = query.Where(expression);
                }
            }

            return await query.CountAsync(cancellationToken);
        }

        public async Task<long> GetLongCountAsync<T>(DbContext dbContext, CancellationToken cancellationToken = default)
            where T : class
        {
            long count = await dbContext.Set<T>().LongCountAsync(cancellationToken);
            return count;
        }

        public async Task<long> GetLongCountAsync<T>(DbContext dbContext, Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default)
            where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return await query.LongCountAsync(cancellationToken);
        }

        public async Task<long> GetLongCountAsync<T>(DbContext dbContext, IEnumerable<Expression<Func<T, bool>>> conditions, CancellationToken cancellationToken = default)
            where T : class
        {
            IQueryable<T> query = dbContext.Set<T>();

            if (conditions != null)
            {
                foreach (Expression<Func<T, bool>> expression in conditions)
                {
                    query = query.Where(expression);
                }
            }

            return await query.LongCountAsync(cancellationToken);
        }

        // DbConext level members
        public async Task<List<T>> GetFromRawSqlAsync<T>(DbContext dbContext, string sql, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            IEnumerable<object> parameters = new List<object>();

            List<T> items = await dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);
            return items;
        }

        public async Task<List<T>> GetFromRawSqlAsync<T>(DbContext dbContext, string sql, object parameter, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            List<object> parameters = new() { parameter };
            List<T> items = await dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);
            return items;
        }

        public async Task<List<T>> GetFromRawSqlAsync<T>(DbContext dbContext, string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new ArgumentNullException(nameof(sql));
            }

            List<T> items = await dbContext.GetFromQueryAsync<T>(sql, parameters, cancellationToken);
            return items;
        }

        public async Task<int> ExecuteSqlCommandAsync(DbContext dbContext, string sql, CancellationToken cancellationToken = default)
        {
            return await dbContext.Database.ExecuteSqlRawAsync(sql, cancellationToken);
        }

        public async Task<int> ExecuteSqlCommandAsync(DbContext dbContext, string sql, params object[] parameters)
        {
            return await dbContext.Database.ExecuteSqlRawAsync(sql, parameters);
        }

        public async Task<int> ExecuteSqlCommandAsync(DbContext dbContext, string sql, IEnumerable<object> parameters, CancellationToken cancellationToken = default)
        {
            return await dbContext.Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
        }

        public void ResetContextState(DbContext dbContext)
        {
            dbContext.ChangeTracker.Clear();
        }

        public void RefreshCache()
        {
            IEnumerable cacheKeys = _cacheService(cacheTech).GetKeys();
            foreach (string key in cacheKeys)
            {
                _cacheService(cacheTech).Remove(key);
            }
        }
    }
}