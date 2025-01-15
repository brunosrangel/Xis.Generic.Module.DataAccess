using MongoDB.Driver;

namespace Xis.Generic.DataAccess.Repository.Interface
{
    public interface IMongoDbContext
    {
        IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : class;
    }
}
