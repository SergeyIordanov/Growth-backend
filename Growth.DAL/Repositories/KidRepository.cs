using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.DAL.Entities;
using Growth.DAL.Interfaces;
using MongoDB.Driver;

namespace Growth.DAL.Repositories
{
    public class KidRepository : IKidRepository
    {
        private readonly IDbContext _context;

        public KidRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kid>> GetByUserAsync(Guid userId)
        {
            var filter = Builders<Kid>.Filter.Eq(kid => kid.UserId, userId);
            var kids = await _context.GetCollection<Kid>().FindAsync(filter);

            return kids.ToEnumerable();
        }

        public async Task<Kid> GetAsync(Guid userId, Guid kidId)
        {
            var filterByUser = Builders<Kid>.Filter.Eq(kid => kid.UserId, userId);
            var filterById = Builders<Kid>.Filter.Eq(kid => kid.Id, kidId);

            var kids = await _context.GetCollection<Kid>().FindAsync(filterByUser & filterById);

            return kids.FirstOrDefault();
        }

        public async Task<Guid> CreateAsync(Guid userId, Kid kid)
        {
            var collection = _context.GetCollection<Kid>();
            kid.Id = Guid.NewGuid();

            await collection.InsertOneAsync(kid);

            return kid.Id;
        }

        public Task DeleteAsync(Guid userId, Guid kidId)
        {
            var collection = _context.GetCollection<Kid>();

            return collection.DeleteOneAsync(entity => entity.UserId.Equals(userId) && entity.Id.Equals(kidId));
        }
    }
}