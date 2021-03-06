﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Growth.BLL.Interfaces;
using Growth.WEB.Models.AccountApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Growth.WEB.Controllers
{
    /// <summary>
    /// Controller for users management
    /// </summary>
    [Route("api/")]
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<UserController> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userService">Instance IUserService interface that provides operations with user</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            this.mapper = mapper;
            this.userService = userService;
            this.logger = logger;
        }

        /// <summary>
        /// Returns all users
        /// </summary>
        [HttpGet("users")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserApiModel), Description = "List of all users")]
        public IActionResult Get()
        {
            var userDtos = userService.GetAll();
            var userApiModels = mapper.Map<IEnumerable<UserApiModel>>(userDtos);

            return Json(userApiModels);
        }

        /// <summary>
        /// Returns user's entity with id
        /// </summary>
        /// <param name="id">User's id</param>
        [HttpGet("users/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserApiModel), Description = "User model")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, typeof(JsonResult), Description = "User with such id does not exist")]
        public async Task<IActionResult> Get(Guid id)
        {
            var userDto = await userService.GetAsync(id);
            var userApiModel = mapper.Map<UserApiModel>(userDto);

            return Json(userApiModel);
        }

        /// <summary>
        /// Adds role for user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleName">Role name</param>
        [HttpPut("users/{userId}/roles/{roleName}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserApiModel), Description = "User's entity with new role")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, typeof(JsonResult), Description = "User with such id or role with such name does not exist")]
        public async Task<IActionResult> AddToRole(Guid userId, string roleName)
        {
            await userService.AddToRoleAsync(userId, roleName);

            var userDto = await userService.GetAsync(userId);
            var userApiModel = mapper.Map<UserApiModel>(userDto);

            logger.LogInformation($"Adding role {roleName} to user {userId}");

            return Ok(userApiModel);
        }

        /// <summary>
        /// Removes role from user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="roleName">Role name</param>
        [HttpDelete("users/{userId}/roles/{roleName}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserApiModel), Description = "User's entity without deleted role")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, typeof(JsonResult), Description = "User with such id or role with such name does not exist")]
        public async Task<IActionResult> RemoveFromRole(Guid userId, string roleName)
        {
            await userService.RemoveRoleAsync(userId, roleName);

            var userDto = await userService.GetAsync(userId);
            var userApiModel = mapper.Map<UserApiModel>(userDto);

            logger.LogInformation($"Remove role {roleName} from user {userId}");

            return Ok(userApiModel);
        }
    }
}