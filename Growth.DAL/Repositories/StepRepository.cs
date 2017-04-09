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
    public class StepRepository : IStepRepository
    {
        private const string IdFieldName = "_id";
        private const int FilteredElementIndex = -1;
        private readonly string _pathCollectionName = new Path().CollectionName;
        private readonly string _goalCollectionName = new Goal().CollectionName;
        private readonly string _stepCollectionName = new Step().CollectionName;
        private readonly IDbContext _context;

        public StepRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Step>> GetByGoalAsync(Guid goalId)
        {
            var filterByPath = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            var projectedKid = await _context.GetCollection<Kid>()
                .Find(filterByPath)
                .Project<Kid>(Builders<Kid>.Projection.Include(t => t.Paths))
                .FirstOrDefaultAsync();

            var path = projectedKid?.Paths?.FirstOrDefault();
            var goal = path?.Goals?.FirstOrDefault();

            if (goal?.Steps == null)
            {
                return new List<Step>();
            }

            return goal.Steps;
        }

        public async Task<Step> GetAsync(Guid goalId, Guid stepId)
        {
            var filterByGoal = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));
            var filterById = Builders<Kid>.Filter
                .Eq(
                    $"{_pathCollectionName}.{_goalCollectionName}.{_stepCollectionName}.{IdFieldName}", 
                    BsonBinaryData.Create(stepId));

            var projectedKid = await _context.GetCollection<Kid>()
                .Find(filterByGoal & filterById)
                .Project<Kid>(Builders<Kid>.Projection.Include(t => t.Paths))
                .FirstOrDefaultAsync();

            var path = projectedKid?.Paths?.FirstOrDefault();
            var goal = path?.Goals?.FirstOrDefault();

            return goal?.Steps?.FirstOrDefault();
        }

        public async Task<Guid> CreateAsync(Guid goalId, Step step)
        {
            // todo
            step.Id = Guid.NewGuid();

            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));
            var update = Builders<Kid>.Update
                .Push($"{_pathCollectionName}.$.{_goalCollectionName}.$.{_stepCollectionName}", step);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);

            return step.Id;
        }

        public async Task<Guid> UpdateAsync(Guid goalId, Step step)
        {
            await DeleteAsync(step.Id);
            await CreateAsync(goalId, step);

            return step.Id;
        }

        public Task DeleteAsync(Guid stepId)
        {
            var update = Builders<Kid>.Update
                .PullFilter(
                    kid => kid.Paths[FilteredElementIndex].Goals[FilteredElementIndex].Steps, 
                    f => f.Id.Equals(stepId));

            var filter = Builders<Kid>.Filter
                .Eq(
                    $"{_pathCollectionName}.{_goalCollectionName}.{_stepCollectionName}.{IdFieldName}", 
                    BsonBinaryData.Create(stepId));

            return _context.GetCollection<Kid>().FindOneAndUpdateAsync(filter, update);
        }
    }
}
