using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.DAL.Entities;

namespace Growth.DAL.Interfaces
{
    public interface IGoalRepository
    {
        Task<IEnumerable<Goal>> GetByPathAsync(Guid pathId);

        Task<Goal> GetAsync(Guid pathId, Guid goalId);

        Task<Guid> CreateAsync(Guid pathId, Goal goal);

        Task<Guid> UpdateAsync(Guid pathId, Goal goal);

        Task DeleteAsync(Guid pathId, Guid goalId);
    }
}