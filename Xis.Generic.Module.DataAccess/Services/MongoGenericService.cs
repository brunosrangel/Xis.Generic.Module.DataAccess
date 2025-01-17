using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Xis.Generic.DataAccess.Repository.Interface;
using Xis.Generic.DataAccess.Service.Interfaces;

namespace Xis.Generic.DataAccess.Service.Services
{
    public class MongoGenericService<TEntity> : IMongoGenericService<TEntity> where TEntity : class
    {
        private readonly IGenericRepositoryMongoDb<TEntity> _repository;
        private readonly ILogger<MongoGenericService<TEntity>> _logger;

        public MongoGenericService(IGenericRepositoryMongoDb<TEntity> repository, ILogger<MongoGenericService<TEntity>> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync() => await ExecuteWithLoggingAsync(
            () => _repository.GetAllAsync(), nameof(GetAllAsync));

        public async Task<TEntity> GetByIdAsync(object id) => await ExecuteWithLoggingAsync(
            () => _repository.GetByIdAsync(id), nameof(GetByIdAsync), new { id });
        public async Task CreateAsync(TEntity entity) => await ExecuteWithLoggingAsync(
    async () => { await _repository.AddAsync(entity); }, nameof(CreateAsync), entity);
        public async Task CreateAsync1(TEntity entity) => await ExecuteWithLoggingAsync(
            () => _repository.AddAsync(entity), nameof(CreateAsync), entity);

        public async Task UpdateAsync(TEntity entity) => await ExecuteWithLoggingAsync(
            () => _repository.UpdateAsync(entity), nameof(UpdateAsync), entity);

        public async Task DeleteAsync(object id) => await ExecuteWithLoggingAsync(
            () => _repository.RemoveAsync(id), nameof(DeleteAsync), new { id });

        public async Task<IEnumerable<TEntity>> QueryAsync(Expression<Func<TEntity, bool>> filter) => await ExecuteWithLoggingAsync(
            () => _repository.QueryAsync(filter), nameof(QueryAsync), new { filter });

        public async Task<TEntity> QuerySingleAsync(Expression<Func<TEntity, bool>> filter) => await ExecuteWithLoggingAsync(
            () => _repository.QuerySingleAsync(filter), nameof(QuerySingleAsync), new { filter });
        private async Task<T> ExecuteWithLoggingAsync<T>(Func<Task<T>> func, string methodName, object? parameters = null)
        {
            try
            {
                return await func();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar o método {MethodName} com os parâmetros {@Parameters}", methodName, parameters);
                throw;
            }
        }

        private async Task ExecuteWithLoggingAsync(Func<Task> func, string methodName, object? parameters = null)
        {
            try
            {
                await func();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao executar o método {MethodName} com os parâmetros {@Parameters}", methodName, parameters);
                throw;
            }
        }
    }
}
