using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.BLL.DTO.Authorization;

namespace Growth.BLL.Interfaces
{
    public interface IUserService
    {
        IEnumerable<UserDto> GetAll();

        Task<UserDto> GetAsync(Guid id);

        Task AddToRoleAsync(Guid userId, string role);

        Task RemoveRoleAsync(Guid userId, string role);

        Task<bool> IsInRoleAsync(Guid userId, string role);
    }
}