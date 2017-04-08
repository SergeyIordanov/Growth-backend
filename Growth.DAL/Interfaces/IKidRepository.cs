using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.DAL.Entities;

namespace Growth.DAL.Interfaces
{
    public interface IKidRepository
    {
        Task<IEnumerable<Kid>> GetByUserAsync(Guid userId);

        Task<Kid> GetAsync(Guid kidId);

        Task<Guid> CreateAsync(Kid kid);

        Task DeleteAsync(Guid kidId);
    }
}