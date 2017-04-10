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
        private readonly string _pathCollectionName = new Path().CollectionName;
        private readonly string _goalCollectionName = new Goal().CollectionName;
        private readonly IDbContext _context;

        public GoalRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Goal>> GetByPathAsync(Guid pathId)
        {
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(pathId));

            var kid = await ReceiveKid(filter);
            var path = kid?.Paths?.FirstOrDefault(p => p.Id == pathId);

            return path?.Goals ?? new List<Goal>();
        }

        public async Task<Goal> GetAsync(Guid pathId, Guid goalId)
        {
            var filterByPath = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(pathId));
            var filterById = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            var kid = await ReceiveKid(filterByPath & filterById);
            var path = kid?.Paths?.FirstOrDefault(p => p.Id == pathId);

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
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goal.Id));

            var kid = await ReceiveKid(filter);
            var path = kid?.Paths?.FirstOrDefault(p => p.Id == pathId);

            if (path?.Goals?.FirstOrDefault() == null)
            {
                return Guid.Empty;
            }

            var goalToUpdate = path.Goals.FirstOrDefault(g => g.Id == goal.Id);
            var goalIndex = path.Goals.IndexOf(goalToUpdate);

            if (goalIndex == -1)
            {
                return Guid.Empty;
            }

            goal.Steps = goalToUpdate?.Steps ?? new List<Step>();

            path.Goals.RemoveAt(goalIndex);
            path.Goals.Insert(goalIndex, goal);

            var update = Builders<Kid>.Update.Set(k => k.Paths, kid.Paths);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);

            return goal.Id;
        }

        public async Task DeleteAsync(Guid pathId, Guid goalId)
        {
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            var kid = await ReceiveKid(filter);

            if (kid?.Paths?.FirstOrDefault() == null)
            {
                return;
            }

            var path = kid.Paths.FirstOrDefault(p => p.Id == pathId);

            if (path?.Goals?.FirstOrDefault() == null)
            {
                return;
            }

            var goalIndex = path.Goals.IndexOf(path.Goals.FirstOrDefault(g => g.Id == goalId));

            if (goalIndex != -1)
            {
                path.Goals.RemoveAt(goalIndex);
            }

            var update = Builders<Kid>.Update.Set(k => k.Paths, kid.Paths);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);
        }

        private async Task<Kid> ReceiveKid(FilterDefinition<Kid> filter)
        {
            var kids = await _context.GetCollection<Kid>().FindAsync(filter);
            var kid = await kids.FirstOrDefaultAsync();

            return kid;
        }
    }
}