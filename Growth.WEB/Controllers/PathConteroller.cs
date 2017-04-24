using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Growth.BLL.DTO;
using Growth.BLL.Interfaces;
using Growth.WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Growth.WEB.Controllers
{
    /// <summary>
    /// Controller for kids management
    /// </summary>
    [Route("api/me/kids/{kidId}/paths")]
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    public class PathController : BaseController
    {
        private readonly IPathService pathService;
        private readonly IMapper mapper;
        private readonly ILogger<KidController> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pathService">Instance IPathService interface that provides operations with paths</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public PathController(IPathService pathService, IMapper mapper, ILogger<KidController> logger)
        {
            this.pathService = pathService;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Returns path with id
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="id">Path id</param>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Path model")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Path with such id for specified kid doesn't exist")]
        public async Task<IActionResult> Get(Guid kidId, Guid id)
        {
            var pathDto = await pathService.GetAsync(kidId, id);
            var pathApiModel = mapper.Map<PathApiModel>(pathDto);

            return Json(pathApiModel);
        }

        /// <summary>
        /// Returns all paths for specified kid
        /// </summary>
        /// <param name="kidId">Kid id</param>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "List of all paths for specified kid")]
        public async Task<IActionResult> Get(Guid kidId)
        {
            var pathDtos = await pathService.GetAllAsync(kidId);
            var pathApiModels = mapper.Map<IEnumerable<PathApiModel>>(pathDtos);

            return Json(pathApiModels);
        }

        /// <summary>
        /// Creates new path
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathApiModel">Path model</param>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid model")]
        public async Task<IActionResult> Post(Guid kidId, [FromBody] PathApiModel pathApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pathDto = mapper.Map<PathDto>(pathApiModel);

            var id = await pathService.CreateAsync(kidId, pathDto);

            logger.LogInformation($"New path {pathApiModel.Title} was created. Id: {id}");

            return Ok(id);
        }

        /// <summary>
        /// Updates a path
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathApiModel">Path model</param>
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid model")]
        public async Task<IActionResult> Put(Guid kidId, [FromBody] PathApiModel pathApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pathDto = mapper.Map<PathDto>(pathApiModel);

            var id = await pathService.UpdateAsync(kidId, pathDto);

            logger.LogInformation($"Path {pathApiModel.Title} was updated. Id: {id}");

            return Ok(id);
        }

        /// <summary>
        /// Deletes path with id 
        /// </summary>
        /// <param name="id">Path id</param>
        /// <param name="kidId">Kid id</param>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Path with such id doesn't exist")]
        public async Task<IActionResult> Delete(Guid kidId, Guid id)
        {
            await pathService.DeleteAsync(kidId, id);

            logger.LogInformation($"Delete path with id: {id}");

            return Ok();
        }
    }
}