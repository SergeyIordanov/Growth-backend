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
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            var kid = await ReceiveKid(filter);
            var goal = ReceiveGoal(goalId, kid);

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

            var kid = await ReceiveKid(filterByGoal & filterById);
            var goal = ReceiveGoal(goalId, kid);

            return goal?.Steps?.FirstOrDefault(s => s.Id == stepId);
        }

        public async Task<Guid> CreateAsync(Guid goalId, Step step)
        {
            step.Id = Guid.NewGuid();

            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            var kid = await ReceiveKid(filter);
            var goal = ReceiveGoal(goalId, kid);

            if (goal == null)
            {
                return Guid.Empty;
            }

            goal.Steps.Add(step);

            var update = Builders<Kid>.Update.Set(k => k.Paths, kid.Paths);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);

            return step.Id;
        }

        public async Task<Guid> UpdateAsync(Guid goalId, Step step)
        {
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            var kid = await ReceiveKid(filter);
            var goal = ReceiveGoal(goalId, kid);

            if (goal?.Steps?.FirstOrDefault() == null)
            {
                return Guid.Empty;
            }

            var stepToUpdate = goal.Steps.FirstOrDefault(s => s.Id == step.Id);
            var stepIndex = goal.Steps.IndexOf(stepToUpdate);

            if (stepIndex == -1)
            {
                return Guid.Empty;
            }

            goal.Steps.RemoveAt(stepIndex);
            goal.Steps.Insert(stepIndex, step);

            var update = Builders<Kid>.Update.Set(k => k.Paths, kid.Paths);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);

            return step.Id;
        }

        public async Task DeleteAsync(Guid goalId, Guid stepId)
        {
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{_goalCollectionName}.{IdFieldName}", BsonBinaryData.Create(goalId));

            var kid = await ReceiveKid(filter);
            var goal = ReceiveGoal(goalId, kid);

            if (goal?.Steps?.FirstOrDefault() == null)
            {
                return;
            }

            var stepIndex = goal.Steps.IndexOf(goal.Steps.FirstOrDefault(s => s.Id == stepId));

            if (stepIndex != -1)
            {
                goal.Steps.RemoveAt(stepIndex);
            }

            var update = Builders<Kid>.Update.Set(k => k.Paths, kid.Paths);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);
        }

        private static Goal ReceiveGoal(Guid goalId, Kid kid)
        {
            var path = kid?.Paths?.FirstOrDefault(p => p.Goals.Any(g => g.Id == goalId));
            var goal = path?.Goals?.FirstOrDefault(g => g.Id == goalId);

            return goal;
        }

        private async Task<Kid> ReceiveKid(FilterDefinition<Kid> filter)
        {
            var kids = await _context.GetCollection<Kid>().FindAsync(filter);
            var kid = await kids.FirstOrDefaultAsync();

            return kid;
        }
    }
}