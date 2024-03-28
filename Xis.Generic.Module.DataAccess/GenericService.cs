using System.Linq.Expressions;
using Xis.Generic.Module.DataAccess.Repository;

namespace Xis.Generic.Module.DataAccess.Service
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;

        public GenericService(IGenericRepository<TEntity> repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return _repository.GetAllAsync();
        }

        public Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.FindAsync(predicate);
        }

        public Task<TEntity> GetByIdAsync(object id)
        {
            return _repository.GetByIdAsync(id);
        }

        public Task CreateAsync(TEntity entity)
        {
            return _repository.AddAsync(entity);
        }

        public Task UpdateAsync(TEntity entity)
        {
            return _repository.UpdateAsync(entity);
        }

        public Task DeleteAsync(object id)
        {
            return _repository.RemoveAsync(id);
        }
        public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _repository.QueryAsync(filter);
        }
        public async Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> filter)
        {
            return await _repository.QuerySingleAsync(filter);
        }
    }
}
