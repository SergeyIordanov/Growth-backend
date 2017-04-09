using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.BLL.DTO;

namespace Growth.BLL.Interfaces
{
    public interface IGoalService
    {
        Task<IEnumerable<GoalDto>> GetAllAsync(Guid kidId, Guid pathId);

        Task<GoalDto> GetAsync(Guid kidId, Guid pathId, Guid goalId);

        Task<Guid> CreateAsync(Guid kidId, Guid pathId, GoalDto goalDto);

        Task<Guid> UpdateAsync(Guid kidId, Guid pathId, GoalDto goalDto);

        Task DeleteAsync(Guid kidId, Guid pathId, Guid goalId);
    }
}
