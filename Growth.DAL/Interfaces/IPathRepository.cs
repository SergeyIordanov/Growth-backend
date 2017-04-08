using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.DAL.Entities;

namespace Growth.DAL.Interfaces
{
    public interface IPathRepository
    {
        Task<IEnumerable<Path>> GetByKidAsync(Guid kidId);

        Task<Path> GetAsync(Guid kidId, Guid pathId);

        Task<Guid> CreateAsync(Guid kidId, Path path);

        Task<Guid> UpdateAsync(Path path);

        Task DeleteAsync(Guid pathId);
    }
}