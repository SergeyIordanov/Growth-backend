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
            var filter = Builders<Kid>.Filter.Eq(kid => kid.Id, BsonBinaryData.Create(kidId));
            var projectedKid = await _context.GetCollection<Kid>()
                .Find(filter)
                .Project<Kid>(Builders<Kid>.Projection.Include(t => t.Paths))
                .FirstOrDefaultAsync();

            return projectedKid == null ? new List<Path>() : projectedKid.Paths;
        }

        public async Task<Path> GetAsync(Guid kidId, Guid pathId)
        {
            var filterByKid = Builders<Kid>.Filter
                .Eq(kid => kid.Id, BsonBinaryData.Create(kidId));
            var filterByPath = Builders<Kid>.Filter
                .Eq($"{_pathCollectionName}.{IdFieldName}", BsonBinaryData.Create(pathId));

            var projectedKid = await _context.GetCollection<Kid>()
                .Find(filterByKid & filterByPath)
                .Project<Kid>(Builders<Kid>.Projection.Include(t => t.Paths))
                .FirstOrDefaultAsync();

            var resultPath = projectedKid?.Paths?.FirstOrDefault(path => path.Id == pathId);

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

            var update = Builders<Kid>.Update
                .Set($"{_pathCollectionName}.$.Title", path.Title)
                .Set($"{_pathCollectionName}.$.Description", path.Description);

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
    }
}