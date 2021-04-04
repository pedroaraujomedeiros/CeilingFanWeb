using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.BaseService
{
    public interface IBaseService<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll(
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
           bool disableTracking = false);

        TEntity GetBy(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool disableTracking = false);

        IQueryable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            bool disableTracking = false);



        TEntity Insert<TValidator>(TEntity obj, string[] ruleSet = null) where TValidator : AbstractValidator<TEntity>;
        TEntity Update<TValidator>(TEntity obj, string[] ruleSet = null) where TValidator : AbstractValidator<TEntity>;
        void Delete(TEntity obj);
        void DeleteRange(IEnumerable<TEntity> objs);
        void DeleteBy(Expression<Func<TEntity, bool>> filter);


        int Count(Expression<Func<TEntity, bool>> filter = null);
        bool Any(Expression<Func<TEntity, bool>> filter = null);

    }
}
