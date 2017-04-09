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
        private const string IdFieldName = "_id";
        private const int FilteredElementIndex = -1;
        private readonly string _pathCollectionName = new Path().CollectionName;
        private readonly string _goalCollectionName = new Goal().CollectionName;
        private readonly IDbContext _context;

        public GoalRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Goal>> GetByPathAsync(Guid pathId)
        {
            var filterByPath = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(pathId));

            var projectedKid = await _context.GetCollection<Kid>()
                .Find(filterByPath)
                .Project<Kid>(Builders<Kid>.Projection.Include(t => t.Paths))
                .FirstOrDefaultAsync();

            var path = projectedKid?.Paths?.FirstOrDefault(p => p.Id == pathId);

            return path?.Goals ?? new List<Goal>();
        }

        public async Task<Goal> GetAsync(Guid pathId, Guid goalId)
        {
            var filterByPath = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(pathId));
            var filterById = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            var projectedKid = await _context.GetCollection<Kid>()
                .Find(filterByPath & filterById)
                .Project<Kid>(Builders<Kid>.Projection.Include(t => t.Paths))
                .FirstOrDefaultAsync();

            var path = projectedKid?.Paths?.FirstOrDefault(p => p.Id == pathId);

            return path?.Goals?.FirstOrDefault(goal => goal.Id == goalId);
        }

        public async Task<Guid> CreateAsync(Guid pathId, Goal goal)
        {
            goal.Id = Guid.NewGuid();

            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(pathId));
            var update = Builders<Kid>.Update
                .Push($"{_pathCollectionName}.$.{_goalCollectionName}", goal);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);

            return goal.Id;
        }

        public async Task<Guid> UpdateAsync(Guid pathId, Goal goal)
        {
            await DeleteAsync(goal.Id);
            await CreateAsync(pathId, goal);

            return goal.Id;
        }

        public Task DeleteAsync(Guid goalId)
        {
            var update = Builders<Kid>.Update
                .PullFilter(kid => kid.Paths[FilteredElementIndex].Goals, f => f.Id.Equals(goalId));
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            return _context.GetCollection<Kid>().FindOneAndUpdateAsync(filter, update);
        }
    }
}