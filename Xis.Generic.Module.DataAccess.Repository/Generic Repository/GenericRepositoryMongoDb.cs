using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Linq.Expressions;
using Xis.Generic.DataAccess.Repository.Interface;

namespace Xis.Generic.DataAccess.Repository
{
    public class GenericRepositoryMongoDb<TEntity> : IGenericRepositoryMongoDb<TEntity> where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly ILogger<GenericRepositoryMongoDb<TEntity>> _logger;

        public GenericRepositoryMongoDb(IMongoCollection<TEntity> collection, ILogger<GenericRepositoryMongoDb<TEntity>> logger)
        {
            _collection = collection;
            _logger = logger;
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            try
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                var entity = await _collection.Find(filter).FirstOrDefaultAsync();
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            try
            {
                return await _collection.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var filter = Builders<TEntity>.Filter.Where(predicate);
                return await _collection.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");
                throw;
            }
        }

        public async Task AddAsync(TEntity entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");
                throw;
            }
        }

        public async Task UpdateAsync(TEntity entity)
        {
            try
            {
                var id = typeof(TEntity).GetProperty("Id")?.GetValue(entity);
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                var update = Builders<TEntity>.Update.Set("Id", id);

                await _collection.ReplaceOneAsync(filter, entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");
                throw;
            }
        }

        public async Task RemoveAsync(object id)
        {
            try
            {
                var filter = Builders<TEntity>.Filter.Eq("_id", id);
                await _collection.DeleteOneAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                var mongoFilter = Builders<TEntity>.Filter.Where(filter);
                return await _collection.Find(mongoFilter).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");
                throw;
            }
        }

        public async Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> filter)
        {
            try
            {
                var mongoFilter = Builders<TEntity>.Filter.Where(filter);
                return await _collection.Find(mongoFilter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");
                throw;
            }
        }

    }
}