using System.Linq.Expressions;

namespace Xis.Generic.Module.DataAccess.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(object id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task RemoveAsync(object id);
        Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter);
        Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> filter);


    }
}
