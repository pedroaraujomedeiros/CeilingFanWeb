using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infra.Repositories.BaseRepository
{
    /// <summary>
    /// Abstract base repository class for the repository layer
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region [Field]

        protected bool disposed = false;
        protected DbContext context;
        protected DbSet<TEntity> dbSet;

        #endregion

        #region [Constructor]

        public BaseRepository(DbContext context)
        {
            this.context = context ?? throw new ArgumentException(nameof(context));
            this.dbSet = this.context.Set<TEntity>();
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get all entries
        /// </summary>
        /// <param name="include">Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>></param>
        /// <param name="disableTracking">bool</param>
        /// <returns>IQueryable<TEntity></returns>
        public virtual IQueryable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = false)
        {
            IQueryable<TEntity> query = dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (include != null) query = include(query);

            return query;
        }

        /// <summary>
        /// Get a single instance based on filters
        /// </summary>
        /// <param name="filter">Expression<Func<TEntity, bool>></param>
        /// <param name="include">Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>></param>
        /// <param name="orderBy">Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>></param>
        /// <param name="disableTracking">bool</param>
        /// <returns>TEntity</returns>
        public virtual TEntity GetBy(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool disableTracking = false)
        {

            IQueryable<TEntity> query = dbSet;

            if (disableTracking) query = query.AsNoTracking();

            if (filter != null) query = query.Where(filter);

            if (include != null) query = include(query);

            if (orderBy != null) query = orderBy(query);

            return query.FirstOrDefault();
        }

        /// <summary>
        /// Get based on filters
        /// </summary>
        /// <param name="filter">Expression<Func<TEntity, bool>></param>
        /// <param name="orderBy">Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>></param>
        /// <param name="include">Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>></param>
        /// <param name="disableTracking">bool</param>
        /// <returns>IQueryable<TEntity></returns>
        public virtual IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool disableTracking = false)
        {
            IQueryable<TEntity> query = dbSet;
            if (disableTracking) query = query.AsNoTracking();

            if (filter != null) query = query.Where(filter);

            if (include != null) query = include(query);

            return orderBy != null ? orderBy(query) : query;

        }

        /// <summary>
        /// Insert an entry
        /// </summary>
        /// <param name="obj">TEntity</param>
        /// <returns>TEntity</returns>
        public virtual TEntity Insert(TEntity obj)
        {
            dbSet.Add(obj);
            return obj;
        }


        /// <summary>
        /// Update an entry
        /// </summary>
        /// <param name="obj">TEntity</param>
        /// <returns>TEntity</returns>
        public virtual TEntity Update(TEntity obj)
        {
            dbSet.Update(obj);
            return obj;
        }


        /// <summary>
        /// Delete an entry
        /// </summary>
        /// <param name="obj">TEntity</param>
        /// <returns></returns>
        public virtual void Delete(TEntity obj)
        {
            dbSet.Remove(obj);
        }


        /// <summary>
        /// Count based on filter
        /// </summary>
        /// <param name="filter">Func<TEntity, bool>></param>
        /// <returns>int</returns>
        public virtual int Count(
            Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.Count();
        }

        /// <summary>
        /// Check if there is any entry based on filter
        /// </summary>
        /// <param name="filter">Func<TEntity, bool>></param>
        /// <returns>bool</returns>
        public virtual bool Any(
           Expression<Func<TEntity, bool>> filter = null)
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.Any();
        }

        /// <summary>
        /// Attach entity
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void Attach(TEntity obj)
        {
            dbSet.Attach(obj);
        }

        /// <summary>
        /// Detach entity
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Detach(TEntity obj)
        {
            context.Entry(obj).State = EntityState.Detached;
        }

        /// <summary>
        /// Get next value for a sequence
        /// </summary>
        /// <param name="sequenceName"></param>
        /// <returns></returns>
        public int NextValueSequence(string sequenceName)
        {
            var p = new SqlParameter("@pSequence", System.Data.SqlDbType.Int);
            p.Direction = System.Data.ParameterDirection.Output;
            context.Database.ExecuteSqlRaw($"SET @pSequence = NEXT VALUE FOR dbo.{sequenceName}", p);
            return (int)p.Value;
        }

        /// <summary>
        /// Execute Raw SQL
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteSqlRaw(string sqlCommand, List<SqlParameter> parameters = null)
        {
            return context.Database.ExecuteSqlRaw(sqlCommand, parameters);
        }


        #endregion
    }
}
