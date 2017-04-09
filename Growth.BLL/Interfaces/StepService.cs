using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.BLL.DTO;

namespace Growth.BLL.Interfaces
{
    public interface IStepService
    {
        Task<IEnumerable<StepDto>> GetAllAsync(Guid kidId, Guid pathId, Guid goalId);

        Task<StepDto> GetAsync(Guid kidId, Guid pathId, Guid goalId, Guid stepId);

        Task<Guid> CreateAsync(Guid kidId, Guid pathId, Guid goalId, StepDto stepDto);

        Task<Guid> UpdateAsync(Guid kidId, Guid pathId, Guid goalId, StepDto stepDto);

        Task DeleteAsync(Guid kidId, Guid pathId, Guid goalId, Guid stepId);
    }
}
