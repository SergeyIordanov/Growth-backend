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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUnitOfWork unitOfWork, IRoleService roleService, IMapper mapper, ILogger<UserService> logger)
        {
            _unitOfWork = unitOfWork;
            _roleService = roleService;
            _mapper = mapper;
            _logger = logger;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = _unitOfWork.Users.GetAll();
            var userDtos = _mapper.Map<List<UserDto>>(users).ToList();

            return userDtos;
        }

        public async Task<UserDto> GetAsync(Guid id)
        {
            var user = await _unitOfWork.Users.GetAsync(id);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    $"User with such id does not exist. Id: {id}", 
                    "User");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }

        public async Task AddToRoleAsync(Guid userId, string role)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);

            if (user == null)
            {
                throw new EntityNotFoundException(
                    $"User with such id does not exist. Id: {userId}",
                    "User");
            }

            var roleDto = _roleService.Get(role);

            user.Roles.Add(roleDto.Name);

            await _unitOfWork.Users.UpdateAsync(user);

            _logger.LogInformation($"Added role {role} to user with id: {userId}");
        }

        public async Task RemoveRoleAsync(Guid userId, string role)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);

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

            await _unitOfWork.Users.UpdateAsync(user);

            _logger.LogInformation($"Removed role {role} from user with id: {userId}");
        }

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);

            return user.Roles.Contains(role);
        }
    }
}