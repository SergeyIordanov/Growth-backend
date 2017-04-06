using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.DAL.Entities;

namespace Growth.DAL.Interfaces
{
    public interface IGoalRepository
    {
        Task<IEnumerable<Goal>> GetByPathAsync(Guid kidId, Guid pathId);

        Task<Goal> GetAsync(Guid kidId, Guid pathId, Guid goalId);

        Task<Guid> CreateAsync(Guid kidId, Guid pathId, Goal goal);

        Task<Guid> UpdateAsync(Guid kidId, Guid pathId, Goal goal);

        Task DeleteAsync(Guid kidId, Guid pathId, Guid goalId);
    }
}