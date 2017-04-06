using System;
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
    /// Controller for roles management
    /// </summary>
    [Route("api/")]
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;
        private readonly ILogger<RoleController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="roleService">Instance IRoleService interface that provides operations with roles</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public RoleController(IRoleService roleService, IMapper mapper, ILogger<RoleController> logger)
        {
            _roleService = roleService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns role with id
        /// </summary>
        /// <param name="id">Role id</param>
        [HttpGet("roles/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Role model")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Role with such id doesn't exist")]
        public async Task<IActionResult> Get(Guid id)
        {
            var roleDto = await _roleService.GetAsync(id);
            var roleApiModel = _mapper.Map<RoleApiModel>(roleDto);

            return Json(roleApiModel);
        }

        /// <summary>
        /// Returns all roles
        /// </summary>
        [HttpGet("roles")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "List of all roles")]
        public IActionResult Get()
        {
            var roleDtos = _roleService.GetAll();
            var roleApiModels = _mapper.Map<IEnumerable<RoleApiModel>>(roleDtos);

            return Json(roleApiModels);
        }

        /// <summary>
        /// Creates new role
        /// </summary>
        /// <param name="roleApiModel">Role model</param>
        [HttpPost("roles")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid model")]
        public async Task<IActionResult> Post([FromBody] RoleApiModel roleApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _roleService.CreateAsync(roleApiModel.Name);

            _logger.LogInformation($"New role {roleApiModel.Name} was created");

            return Ok();
        }

        /// <summary>
        /// Deletes role with id 
        /// </summary>
        /// <param name="id">Role id</param>
        [HttpDelete("roles/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Role with such id doesn't exist")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _roleService.DeleteAsync(id);

            _logger.LogInformation($"Delete role with id: {id}");

            return Ok();
        }
    }
}