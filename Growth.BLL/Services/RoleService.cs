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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleService> _logger;

        public RoleService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<RoleService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<RoleDto> GetAll()
        {
            var roles = _unitOfWork.Roles.GetAll();
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);

            _logger.LogInformation("Get all roles");

            return roleDtos;
        }

        public async Task<RoleDto> GetAsync(Guid id)
        {
            var role = await _unitOfWork.Roles.GetAsync(id);

            if (role == null)
            {
                throw new EntityNotFoundException(
                    $"Role with such id does not exist. Id: {id}",
                    "Role");
            }

            var roleDto = _mapper.Map<RoleDto>(role);

            return roleDto;
        }

        public RoleDto Get(string name)
        {
            var role = _unitOfWork.Roles
                .Find(r => r.Name.Equals(name))
                .FirstOrDefault();

            if (role == null)
            {
                throw new EntityNotFoundException(
                    $"Role with such name does not exist. Name: {name}",
                    "Role");
            }

            var roleDto = _mapper.Map<RoleDto>(role);

            return roleDto;
        }

        public async Task CreateAsync(string name)
        {
            var role = _unitOfWork.Roles
                .Find(r => r.Name.Equals(name))
                .FirstOrDefault();

            if (role != null)
            {
                throw new EntityExistsException(
                    $"Role with such name already exists. Name: {name}",
                    "Role");
            }

            var roleToCreate = new Role { Name = name };

            await _unitOfWork.Roles.CreateAsync(roleToCreate);

            _logger.LogInformation($"New role {name} created");
        }

        public async Task DeleteAsync(Guid id)
        {
            var role = await _unitOfWork.Roles.GetAsync(id);

            if (role == null)
            {
                throw new EntityNotFoundException(
                    $"Role with such id does not exist. Id: {id}",
                    "Role");
            }

            await _unitOfWork.Roles.DeleteAsync(id);

            _logger.LogInformation($"Role with id: {id} was deleted");
        }
    }
}