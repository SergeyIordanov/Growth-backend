using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Growth.BLL.DTO.Authorization;
using Growth.BLL.Infrastructure.Exceptions;
using Growth.BLL.Interfaces;
using Growth.DAL.Entities;
using Growth.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growth.BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ILogger<RoleService> logger;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RoleService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.logger = logger;
        }

        public IEnumerable<RoleDto> GetAll()
        {
            var roles = unitOfWork.Roles.GetAll();
            var roleDtos = mapper.Map<IEnumerable<RoleDto>>(roles);

            logger.LogInformation("Get all roles");

            return roleDtos;
        }

        public async Task<RoleDto> GetAsync(Guid id)
        {
            var role = await unitOfWork.Roles.GetAsync(id);

            if (role == null)
            {
                throw new EntityNotFoundException(
                    $"Role with such id does not exist. Id: {id}",
                    "Role");
            }

            var roleDto = mapper.Map<RoleDto>(role);

            return roleDto;
        }

        public RoleDto Get(string name)
        {
            var role = unitOfWork.Roles
                .Find(r => r.Name.Equals(name))
                .FirstOrDefault();

            if (role == null)
            {
                throw new EntityNotFoundException(
                    $"Role with such name does not exist. Name: {name}",
                    "Role");
            }

            var roleDto = mapper.Map<RoleDto>(role);

            return roleDto;
        }

        public async Task CreateAsync(string name)
        {
            var role = unitOfWork.Roles
                .Find(r => r.Name.Equals(name))
                .FirstOrDefault();

            if (role != null)
            {
                throw new EntityExistsException(
                    $"Role with such name already exists. Name: {name}",
                    "Role");
            }

            var roleToCreate = new Role { Name = name };

            await unitOfWork.Roles.CreateAsync(roleToCreate);

            logger.LogInformation($"New role {name} created");
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await unitOfWork.Roles.GetAsync(id);

            if (role == null)
            {
                throw new EntityNotFoundException(
                    $"Role with such id does not exist. Id: {id}",
                    "Role");
            }

            await unitOfWork.Roles.DeleteAsync(id);

            logger.LogInformation($"Role with id: {id} was deleted");
        }
    }
}