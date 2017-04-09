using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.DAL.Entities;

namespace Growth.DAL.Interfaces
{
    public interface IStepRepository
    {
        Task<IEnumerable<Step>> GetByGoalAsync(Guid goalId);

        Task<Step> GetAsync(Guid goalId, Guid stepId);

        Task<Guid> CreateAsync(Guid goalId, Step step);

        Task<Guid> UpdateAsync(Guid goalId, Step step);

        Task DeleteAsync(Guid stepId);
    }
}
