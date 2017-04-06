using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Growth.BLL.DTO.Authorization;

namespace Growth.BLL.Interfaces
{
    public interface IRoleService
    {
        IEnumerable<RoleDto> GetAll();

        Task<RoleDto> GetAsync(Guid id);

        RoleDto Get(string name);

        Task CreateAsync(string name);

        Task DeleteAsync(Guid id);
    }
}