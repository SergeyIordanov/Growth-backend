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
    public class PathRepository : IPathRepository
    {
        private const string IdFieldName = "_id";
        private readonly string _pathCollectionName = new Path().CollectionName;
        private readonly IDbContext _context;

        public PathRepository(IDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Path>> GetByKidAsync(Guid kidId)
        {
            var filter = Builders<Kid>.Filter.Eq(k => k.Id, BsonBinaryData.Create(kidId));
            var kid = await ReceiveKid(filter);

            return kid?.Paths ?? new List<Path>();
        }

        public async Task<Path> GetAsync(Guid kidId, Guid pathId)
        {
            var filterByKid = Builders<Kid>.Filter
                .Eq(k => k.Id, BsonBinaryData.Create(kidId));
            var filterByPath = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(pathId));

            var kid = await ReceiveKid(filterByKid & filterByPath);

            var resultPath = kid?.Paths?.FirstOrDefault(path => path.Id == pathId);

            return resultPath;
        }

        public async Task<Guid> CreateAsync(Guid kidId, Path path)
        {
            path.Id = Guid.NewGuid();
            var filter = Builders<Kid>.Filter.Eq(kid => kid.Id, BsonBinaryData.Create(kidId));
            var update = Builders<Kid>.Update.Push(t => t.Paths, path);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);

            return path.Id;
        }

        public async Task<Guid> UpdateAsync(Path path)
        {
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(path.Id));

            var kid = await ReceiveKid(filter);
            var pathToUpdate = kid?.Paths?.FirstOrDefault(p => p.Id == path.Id);
            var pathIndex = kid?.Paths?.IndexOf(pathToUpdate);

            if (!pathIndex.HasValue || pathIndex.Value == -1)
            {
                return Guid.Empty;
            }

            path.Goals = pathToUpdate?.Goals ?? new List<Goal>();

            kid.Paths.RemoveAt(pathIndex.Value);
            kid.Paths.Insert(pathIndex.Value, path);

            var update = Builders<Kid>.Update.Set(k => k.Paths, kid.Paths);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);

            return path.Id;
        }

        public Task DeleteAsync(Guid pathId)
        {
            var update = Builders<Kid>.Update
                .PullFilter(kid => kid.Paths, path => path.Id.Equals(pathId));
            var filter = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(pathId));

            return _context.GetCollection<Kid>().FindOneAndUpdateAsync(filter, update);
        }

        private async Task<Kid> ReceiveKid(FilterDefinition<Kid> filter)
        {
            var kids = await _context.GetCollection<Kid>().FindAsync(filter);
            var kid = await kids.FirstOrDefaultAsync();

            return kid;
        }
    }
}