using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.DAL.Entities;

namespace Growth.DAL.Interfaces
{
    public interface IKidRepository
    {
        Task<IEnumerable<Kid>> GetByUserAsync(Guid userId);

        Task<Kid> GetAsync(Guid userId, Guid kidId);

        Task<Guid> CreateAsync(Guid userId, Kid kid);

        Task DeleteAsync (Guid userId, Guid kidId);
    }
}