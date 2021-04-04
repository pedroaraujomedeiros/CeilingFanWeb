using Infra.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Infra.UnitOfWork
{
    /// <summary>
    /// Unit of Work class
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public class UnitOfWork<TContext> :
            IRepositoryFactory,
            IUnitOfWork<TContext> where TContext : DbContext, IDisposable
    {

        #region [Field]

        private bool disposed = false;
        public TContext Context { get; }
        private Dictionary<Type, object> repositories;

        #endregion

        #region [Constructor]
        public UnitOfWork()
        {
            this.Context = (TContext)Activator.CreateInstance(typeof(TContext));
        }

        public UnitOfWork(TContext context)
        {
            this.Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get repository
        /// </summary>
        /// <typeparam name="TRepository"></typeparam>
        /// <returns>TRepository</returns>
        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            if (repositories == null) repositories = new Dictionary<Type, object>();

            var type = typeof(TEntity);
            if (!repositories.ContainsKey(type)) repositories[type] = new BaseRepository<TEntity>(Context);
            return (IRepository<TEntity>)repositories[type];
        }



        /// <summary>
        /// SaveChanges method if want to save token data.
        /// </summary>
        /// <param name="tokenInfo"></param>
        /// <returns></returns>
        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new ValidationException(ex.Message);
            }
        }


        /// <summary>
        /// Default IDispose implementation
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    Context?.Dispose();
                }
            }
            this.disposed = true;
        }

        /// <summary>
        /// Default IDispose implementation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
