using Infra.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infra.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;
        int SaveChanges();
    }

    public interface IUnitOfWork<TContext> : IUnitOfWork
        where TContext : DbContext
    {
        TContext Context { get; }
    }
}
