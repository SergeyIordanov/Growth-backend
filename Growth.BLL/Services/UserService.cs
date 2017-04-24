using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Growth.BLL.DTO.Authorization;
using Growth.BLL.Infrastructure.Exceptions;
using Growth.BLL.Interfaces;
using Growth.DAL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Growth.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRoleService roleService;
        private readonly IMapper mapper;
        private readonly ILogger<UserService> logger;

        public UserService(IUnitOfWork unitOfWork, IRoleService roleService, IMapper mapper, ILogger<UserService> logger)
        {
            this.unitOfWork = unitOfWork;
            this.roleService = roleService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = unitOfWork.Users.GetAll();
            var userDtos = mapper.Map<List<UserDto>>(users).ToList();

            return userDtos;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await unitOfWork.Users.GetAsync(id);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    $"User with such id does not exist. Id: {id}", 
                    "User");
            }

            var userDto = mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task AddToRoleAsync(Guid userId, string role)
        {
            var user = await unitOfWork.Users.GetAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    $"User with such id does not exist. Id: {userId}",
                    "User");
            }

            var roleDto = roleService.Get(role);

            user.Roles.Add(roleDto.Name);

            await unitOfWork.Users.UpdateAsync(user);

            logger.LogInformation($"Added role {role} to user with id: {userId}");
        }

        public async Task RemoveRoleAsync(Guid userId, string role)
        {
            var user = await unitOfWork.Users.GetAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    $"User with such id does not exist. Id: {userId}",
                    "User");
            }

            if (!user.Roles.Contains(role))
            {
                throw new EntityNotFoundException(
                    $"User isn't in specified role. Role: {role}",
                    "Role");
            }

            user.Roles = user.Roles.Where(roleName => !roleName.Equals(role)).ToList();

            await unitOfWork.Users.UpdateAsync(user);

            logger.LogInformation($"Removed role {role} from user with id: {userId}");
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = await unitOfWork.Users.GetAsync(userId);

            return user.Roles.Contains(role);
        }
    }
}