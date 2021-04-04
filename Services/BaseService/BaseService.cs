using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Infra.Repositories.BaseRepository;
using Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.BaseService
{
    /// <summary>
    /// Abstract base service class for the service layer
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class BaseService<TEntity> : IBaseService<TEntity>
            where TEntity : class

    {
        #region [Fields]
        protected readonly IUnitOfWork uow;
        protected BaseRepository<TEntity> repo;

        #endregion

        #region [Constructor]
        //public BaseService()
        //{
        //    this.uow = new UnitOfWork<DbContext>();
        //    this.repo = uow.GetRepository<TEntity>() as BaseRepository<TEntity>;
        //}

        public BaseService(IUnitOfWork uow)
        {
            this.uow = uow as IUnitOfWork;
            this.repo = uow.GetRepository<TEntity>() as BaseRepository<TEntity>;
        }

        #endregion

        #region [Methods]

        /// <summary>
        /// Get all entries
        /// </summary>
        /// <param name="include">Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>></param>
        /// <param name="disableTracking">bool</param>
        /// <returns>IQueryable<TEntity></returns>
        public IQueryable<TEntity> GetAll(
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = false)
        {
            return repo.GetAll(include, disableTracking);
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
            return repo.Get(filter, include, orderBy, disableTracking);
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
            return repo.GetBy(filter, include, orderBy, disableTracking);
        }



        /// <summary>
        /// Insert an entry
        /// </summary>
        /// <typeparam name="TValidator"></typeparam>
        /// <param name="instance"></param>
        /// <param name="ruleSet"></param>
        /// <returns>TEntity</returns>
        public virtual TEntity Insert<TValidator>(TEntity instance, string[] ruleSet = null) where TValidator : AbstractValidator<TEntity>
        {
            if (instance == null)
                throw new ArgumentException("Entity does not exist");

            //Get repository
            var repo = this.uow.GetRepository<TEntity>();

            //Validate instance
            Validate(instance, Activator.CreateInstance<TValidator>(), ruleSet);

            instance = repo.Insert(instance);

            uow.SaveChanges();

            return instance;
        }

        /// <summary>
        /// Update an entry
        /// </summary>
        /// <typeparam name="TValidator"></typeparam>
        /// <param name="instance"></param>
        /// <param name="ruleSet"></param>
        /// <returns>TEntity</returns>
        public virtual TEntity Update<TValidator>(TEntity instance, string[] ruleSet = null) where TValidator : AbstractValidator<TEntity>
        {
            if (instance == null)
                throw new ArgumentException("Entity does not exist");

            //Get repository
            var repo = this.uow.GetRepository<TEntity>();

            //Validate instance
            Validate(instance, Activator.CreateInstance<TValidator>(), ruleSet);

            instance = repo.Update(instance);

            uow.SaveChanges();

            return instance;
        }

        /// <summary>
        /// Delete an entry
        /// </summary>
        /// <param name="instance">TEntity</param>
        public void Delete(TEntity instance)
        {
            if (instance == null)
                throw new ArgumentException("Entity does not exist");

            //Get repository
            var repo = this.uow.GetRepository<TEntity>();

            repo.Delete(instance);

            uow.SaveChanges();

        }

        /// <summary>
        /// Delete a list of instances
        /// </summary>
        /// <param name="objs">IEnumerable<TEntity></param>
        public void DeleteRange(IEnumerable<TEntity> objs)
        {
            if (objs == null)
                throw new ArgumentException("Entity does not exist");

            foreach (TEntity instance in objs)
            {
                Delete(instance);
            }
        }

        /// <summary>
        /// Delete based on a filter
        /// </summary>
        /// <param name="filter">Expression<Func<TEntity, bool>></param>
        public void DeleteBy(Expression<Func<TEntity, bool>> filter)
        {
            TEntity entity = repo.GetBy(filter, null, null, false);
            if (entity != null)
                Delete(entity);
        }

        /// <summary>
        /// Count based on filter
        /// </summary>
        /// <param name="filter">Func<TEntity, bool>></param>
        /// <returns>int</returns>
        public int Count(
            Expression<Func<TEntity, bool>> filter = null)
        {
            return repo.Count(filter);
        }

        /// <summary>
        /// Check if there is any entry based on filter
        /// </summary>
        /// <param name="filter">Func<TEntity, bool>></param>
        /// <returns>bool</returns>
        public bool Any(
           Expression<Func<TEntity, bool>> filter = null)
        {
            return repo.Any(filter);
        }

        /// <summary>
        /// Detach an instance
        /// </summary>
        /// <param name="instance"></param>
        internal void Detach(TEntity instance)
        {
            if (instance != null)
                repo.Detach(instance);
        }


        /// <summary>
        /// Validate an instance
        /// </summary>
        /// <typeparam name="TValidator"></typeparam>
        /// <param name="instance"></param>
        /// <param name="ruleSet"></param>
        /// <returns>ValidationResult</returns>
        private ValidationResult Validate<TValidator>(TEntity instance, string[] ruleSet = null) where TValidator : AbstractValidator<TEntity>
        {
            return Validate(instance, Activator.CreateInstance<TValidator>(), ruleSet);
        }

        /// <summary>
        /// Validate an instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="validator"></param>
        /// <param name="ruleSet"></param>
        /// <returns>ValidationResult</returns>
        private ValidationResult Validate(TEntity instance, AbstractValidator<TEntity> validator, string[] ruleSet = null)
        {
            ruleSet = ruleSet ?? new string[] { "*" };
            if (instance == null)
                throw new Exception("No detected registers!");

            return validator.Validate(instance, options => options.IncludeRuleSets(ruleSet).ThrowOnFailures());
        }

        #endregion
    }
}
