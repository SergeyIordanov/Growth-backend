using Growth.DAL.Entities;
using MongoDB.Driver;

namespace Growth.DAL.Interfaces
{
    public interface IDbContext
    {
        IMongoCollection<TEntity> GetCollection<TEntity>() where TEntity : BaseType, new();
    }
}