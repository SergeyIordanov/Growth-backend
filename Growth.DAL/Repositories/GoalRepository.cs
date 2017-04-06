using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Growth.DAL.Entities;
using Growth.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Growth.DAL.Repositories
{
    public class GoalRepository : IGoalRepository
    {
        private const int FilteredElementIndex = -1;
        private readonly IDbContext _context;

        public GoalRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Goal>> GetByPathAsync(Guid kidId, Guid pathId)
        {
            var filterByKid = Builders<Kid>.Filter.Eq(kid => kid.Id, BsonBinaryData.Create(kidId));
            var filterByPath = Builders<Kid>.Filter.Eq(
                kid => kid.Paths.ElementAtOrDefault(FilteredElementIndex) != null
                    ? kid.Paths.ElementAt(FilteredElementIndex).Id
                    : Guid.Empty,
                pathId);

            var projectedKid = await _context.GetCollection<Kid>()
                .Find(filterByKid & filterByPath).FirstOrDefaultAsync();

            if (projectedKid?.Paths?.FirstOrDefault() == null)
            {
                return new List<Goal>();
            }

            return projectedKid.Paths.First().Goals;
        }

        public Task<Goal> GetAsync(Guid kidId, Guid pathId, Guid goalId)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> CreateAsync(Guid kidId, Guid pathId, Goal goal)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> UpdateAsync(Guid kidId, Guid pathId, Goal goal)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Guid kidId, Guid pathId, Guid goalId)
        {
            throw new NotImplementedException();
        }
    }
}