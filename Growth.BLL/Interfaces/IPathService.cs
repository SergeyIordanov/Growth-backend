using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.BLL.DTO;

namespace Growth.BLL.Interfaces
{
    public interface IPathService
    {
        Task<IEnumerable<PathDto>> GetAllAsync(Guid kidId);

        Task<PathDto> GetAsync(Guid kidId, Guid pathId);

        Task<Guid> CreateAsync(Guid kidId, PathDto pathDto);

        Task<Guid> UpdateAsync(Guid kidId, PathDto pathDto);

        Task DeleteAsync(Guid kidId, Guid pathId);
    }
}
