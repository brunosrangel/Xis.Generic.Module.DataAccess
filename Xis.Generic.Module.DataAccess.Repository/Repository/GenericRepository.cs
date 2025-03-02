﻿
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using Xis.Generic.DataAccess.Repository.Interface;

namespace Xis.Generic.DataAccess.Repository.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly ILogger<GenericRepository<TEntity>> _logger;

        public GenericRepository(DbContext context, ILogger<GenericRepository<TEntity>> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<TEntity>();
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<TEntity?> GetByIdAsync(object id)
        {
            try
            {
                return await _dbSet.FindAsync(id);
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
                return await Task.FromResult(_dbSet.AsEnumerable());
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
                return await Task.FromResult(_dbSet.Where(predicate).AsEnumerable());
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
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
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
                _dbSet.Update(entity);
                await _context.SaveChangesAsync();
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
                var objEntity = await GetByIdAsync(id);
                if (objEntity != null)
                {
                    _dbSet.Remove(objEntity);
                    await _context.SaveChangesAsync();
                }
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
                return await _dbSet.Where(filter).ToListAsync();
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
                return await _dbSet.FirstOrDefaultAsync(filter);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro na solicitação");

                throw;
            }
        }

        Task<TEntity> IGenericRepository<TEntity>.GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IGenericRepository<TEntity>.GetAllAsync()
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IGenericRepository<TEntity>.FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<TEntity>.AddAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<TEntity>.UpdateAsync(TEntity entity)
        {
            throw new NotImplementedException();
        }

        Task IGenericRepository<TEntity>.RemoveAsync(object id)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<TEntity>> IGenericRepository<TEntity>.QueryAsync(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }

        Task<TEntity> IGenericRepository<TEntity>.QuerySingleAsync(Expression<Func<TEntity, bool>> filter)
        {
            throw new NotImplementedException();
        }
    }
}