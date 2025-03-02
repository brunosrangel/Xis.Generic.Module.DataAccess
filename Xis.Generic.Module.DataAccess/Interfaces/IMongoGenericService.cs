﻿using System.Linq.Expressions;

namespace Xis.Generic.DataAccess.Service.Interfaces
{
    public interface IMongoGenericService<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity?> GetByIdAsync(object id);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(object id);
        Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity?> QuerySingleAsync(Expression<Func<TEntity, bool>> filter);

    }
}