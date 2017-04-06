using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.BLL.DTO;

namespace Growth.BLL.Interfaces
{
    public interface IKidService
    {
        Task<IEnumerable<KidDto>> GetAllAsync(Guid userId);

        Task<KidDto> GetAsync(Guid userId, Guid kidId);

        Task<Guid> CreateAsync(Guid userId, KidDto kidDto);

        Task DeleteAsync(Guid userId, Guid kidId);
    }
}