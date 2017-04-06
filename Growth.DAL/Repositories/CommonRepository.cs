using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Growth.DAL.Entities;
using Growth.DAL.Interfaces;
using MongoDB.Driver;

namespace Growth.DAL.Repositories
{
    public class CommonRepository<TEntity> : IRepository<TEntity> where TEntity : BaseType, new()
    {
        protected readonly IDbContext Context;

        public CommonRepository(IDbContext context)
        {
            Context = context;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            var collection = Context.GetCollection<TEntity>();

            return collection.AsQueryable();
        }

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            var collection = Context.GetCollection<TEntity>();
            var entity = (await collection.FindAsync(e => e.Id.Equals(id))).FirstOrDefault();

            return entity;
        }

        public virtual IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> expression)
        {
            var collection = Context.GetCollection<TEntity>();
            var filterCompile = expression.Compile();
            var entities = collection.AsQueryable().Where(filterCompile);

            return entities.AsQueryable();
        }

        public virtual IQueryable<TEntity> Find(int skip, int take, Expression<Func<TEntity, bool>> expression = null)
        {
            var collection = Context.GetCollection<TEntity>();
            IQueryable<TEntity> entities = collection.AsQueryable();

            if (expression != null)
            {
                var filterCompile = expression.Compile();
                entities = entities.Where(filterCompile).AsQueryable();
            }

            entities = entities.Skip(skip).Take(take);

            return entities;
        }

        public virtual async Task CreateAsync(TEntity item)
        {
            var collection = Context.GetCollection<TEntity>();
            item.Id = Guid.NewGuid();

            await collection.InsertOneAsync(item);
        }

        public async Task UpdateAsync(TEntity item)
        {
            var collection = Context.GetCollection<TEntity>();

            await collection.ReplaceOneAsync(entity => entity.Id.Equals(item.Id), item);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var collection = Context.GetCollection<TEntity>();

            await collection.DeleteOneAsync(entity => entity.Id.Equals(id));
        }
    }
}