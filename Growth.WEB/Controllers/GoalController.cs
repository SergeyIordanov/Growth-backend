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
    /// Controller for goals management
    /// </summary>
    [Route("api/me/kids/{kidId}/paths/{pathId}/goals")]
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    public class GoalController : BaseController
    {
        private readonly IGoalService _goalService;
        private readonly IMapper _mapper;
        private readonly ILogger<KidController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="goalService">Instance IGoalService interface that provides operations with goals</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public GoalController(IGoalService goalService, IMapper mapper, ILogger<KidController> logger)
        {
            _goalService = goalService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Returns goal with id
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="id">Goal id</param>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Goal model")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Goal with such id for specified path doesn't exist")]
        public async Task<IActionResult> Get(Guid kidId, Guid pathId, Guid id)
        {
            var goalDto = await _goalService.GetAsync(kidId, pathId, id);
            var goalApiModel = _mapper.Map<GoalApiModel>(goalDto);

            return Json(goalApiModel);
        }

        /// <summary>
        /// Returns all goals for specified path
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "List of all goals for specified path")]
        public async Task<IActionResult> Get(Guid kidId, Guid pathId)
        {
            var goalDtos = await _goalService.GetAllAsync(kidId, pathId);
            var goalApiModels = _mapper.Map<IEnumerable<GoalApiModel>>(goalDtos);

            return Json(goalApiModels);
        }

        /// <summary>
        /// Creates new goal
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="goalApiModel">Goal model</param>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid model")]
        public async Task<IActionResult> Post(Guid kidId, Guid pathId, [FromBody] GoalApiModel goalApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var goalDto = _mapper.Map<GoalDto>(goalApiModel);

            var id = await _goalService.CreateAsync(kidId, pathId, goalDto);

            _logger.LogInformation($"New goal {goalApiModel.Title} was created. Id: {id}");

            return Ok(id);
        }

        /// <summary>
        /// Updates a goal
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="goalApiModel">Goal model</param>
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid model")]
        public async Task<IActionResult> Put(Guid kidId, Guid pathId, [FromBody] GoalApiModel goalApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var goalDto = _mapper.Map<GoalDto>(goalApiModel);

            var id = await _goalService.UpdateAsync(kidId, pathId, goalDto);

            _logger.LogInformation($"Goal {goalApiModel.Title} was updated. Id: {id}");

            return Ok(id);
        }

        /// <summary>
        /// Deletes goal with id 
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="id">Goal id</param>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Goal with such id doesn't exist")]
        public async Task<IActionResult> Delete(Guid kidId, Guid pathId, Guid id)
        {
            await _goalService.DeleteAsync(kidId, pathId, id);

            _logger.LogInformation($"Delete goal with id: {id}");

            return Ok();
        }
    }
}