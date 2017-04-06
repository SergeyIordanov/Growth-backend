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
        private const int FilteredElementIndex = -1;
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
            var filterByKid = Builders<Kid>.Filter.Eq(kid => kid.Id, BsonBinaryData.Create(kidId));
            var filterById = Builders<Kid>.Filter.Eq(
                kid => kid.Paths.ElementAtOrDefault(FilteredElementIndex) != null
                    ? kid.Paths.ElementAt(FilteredElementIndex).Id
                    : Guid.Empty,
                pathId);

            var projectedKid = await _context.GetCollection<Kid>()
                .Find(filterByKid & filterById)
                .Project<Kid>(Builders<Kid>.Projection.Include(t => t.Paths))
                .FirstOrDefaultAsync();

            return projectedKid?.Paths?.FirstOrDefault();
        }

        public async Task<Guid> CreateAsync(Guid kidId, Path path)
        {
            path.Id = Guid.NewGuid();
            var filter = Builders<Kid>.Filter.Eq(kid => kid.Id, BsonBinaryData.Create(kidId));
            var update = Builders<Kid>.Update.Push(t => t.Paths, path);

            await _context.GetCollection<Kid>().UpdateOneAsync(filter, update);

            return path.Id;
        }

        public async Task<Guid> UpdateAsync(Guid kidId, Path path)
        {
            var filterByKid = Builders<Kid>.Filter.Eq(kid => kid.Id, BsonBinaryData.Create(kidId));
            var filterById = Builders<Kid>.Filter.Eq(
                kid => kid.Paths.ElementAtOrDefault(FilteredElementIndex) != null
                    ? kid.Paths.ElementAt(FilteredElementIndex).Id
                    : Guid.Empty,
                path.Id);

            var update = Builders<Kid>.Update
                .Set(x => x.Paths.ElementAtOrDefault(FilteredElementIndex), path);

            await _context.GetCollection<Kid>().UpdateOneAsync(filterByKid & filterById, update);

            return path.Id;
        }

        public Task DeleteAsync(Guid kidId, Guid pathId)
        {
            var filterByKid = Builders<Kid>.Filter.Eq(kid => kid.Id, BsonBinaryData.Create(kidId));
            var filterById = Builders<Kid>.Filter.Eq(
                kid => kid.Paths.ElementAtOrDefault(FilteredElementIndex) != null
                    ? kid.Paths.ElementAt(FilteredElementIndex).Id
                    : Guid.Empty,
                pathId);

            var update = Builders<Kid>.Update.PullFilter(p => p.Paths, f => f.Id.Equals(pathId));

            return _context.GetCollection<Kid>().FindOneAndUpdateAsync(filterByKid & filterById, update);
        }
    }
}