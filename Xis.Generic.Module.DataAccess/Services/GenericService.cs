using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Xis.Generic.DataAccess.Repository.Interface;
using Xis.Generic.DataAccess.Service.Interfaces;


namespace Xis.Generic.DataAccess.Service.Services
{
    public class GenericService<TEntity> : IGenericService<TEntity> where TEntity : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly ILogger<GenericService<TEntity>> _logger;

        public GenericService(IGenericRepository<TEntity> repository, ILogger<GenericService<TEntity>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all entities");
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _repository.FindAsync(predicate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while finding entities with predicate: {predicate}", predicate);
                throw;
            }
        }

        public async Task<TEntity> GetByIdAsync(object id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting entity by ID: {id}", id);
                throw;
            }
        }

        public async Task CreateAsync(TEntity entity)
        {
            try
            {
                await _repository.AddAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating entity: {@entity}", entity);
                throw;
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                await _repository.UpdateAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating entity: {@entity}", entity);
                throw;
            }
        }

        public async Task DeleteAsync(object id)
        {
            try
            {
                await _repository.RemoveAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting entity with ID: {id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return await _repository.QueryAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while querying entities with filter: {filter}", filter);
                throw;
            }
        }

        public async Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                return await _repository.QuerySingleAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while querying a single entity with filter: {filter}", filter);
                throw;
            }
        }
    }
}