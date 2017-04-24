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
    /// Controller for steps management
    /// </summary>
    [Route("api/me/kids/{kidId}/paths/{pathId}/goals/{goalId}/steps")]
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    public class StepController : BaseController
    {
        private readonly IStepService stepService;
        private readonly IMapper mapper;
        private readonly ILogger<KidController> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stepService">Instance IStepService interface that provides operations with steps</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public StepController(IStepService stepService, IMapper mapper, ILogger<KidController> logger)
        {
            this.stepService = stepService;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Returns step with id
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="goalId">Goal id</param>
        /// <param name="id">Step id</param>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Step model")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Step with such id for specified goal doesn't exist")]
        public async Task<IActionResult> Get(Guid kidId, Guid pathId, Guid goalId, Guid id)
        {
            var stepDto = await stepService.GetAsync(kidId, pathId, goalId, id);
            var stepApiModel = mapper.Map<StepApiModel>(stepDto);

            return Json(stepApiModel);
        }

        /// <summary>
        /// Returns all steps for specified goal
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="goalId">Goal id</param>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "List of all steps for specified goal")]
        public async Task<IActionResult> Get(Guid kidId, Guid pathId, Guid goalId)
        {
            var stepDtos = await stepService.GetAllAsync(kidId, pathId, goalId);
            var stepApiModels = mapper.Map<IEnumerable<StepApiModel>>(stepDtos);

            return Json(stepApiModels);
        }

        /// <summary>
        /// Creates new step
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="goalId">Goal id</param>
        /// <param name="stepApiModel">Step model</param>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid model")]
        public async Task<IActionResult> Post(Guid kidId, Guid pathId, Guid goalId, [FromBody] StepApiModel stepApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stepDto = mapper.Map<StepDto>(stepApiModel);

            var id = await stepService.CreateAsync(kidId, pathId, goalId, stepDto);

            logger.LogInformation($"New step {stepApiModel.Text} was created. Id: {id}");

            return Ok(id);
        }

        /// <summary>
        /// Updates a step
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="goalId">Goal id</param>
        /// <param name="stepApiModel">Step model</param>
        [HttpPut]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid model")]
        public async Task<IActionResult> Put(Guid kidId, Guid pathId, Guid goalId, [FromBody] StepApiModel stepApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var stepDto = mapper.Map<StepDto>(stepApiModel);

            var id = await stepService.UpdateAsync(kidId, pathId, goalId, stepDto);

            logger.LogInformation($"Step {stepApiModel.Text} was updated. Id: {id}");

            return Ok(id);
        }

        /// <summary>
        /// Deletes step with id 
        /// </summary>
        /// <param name="kidId">Kid id</param>
        /// <param name="pathId">Path id</param>
        /// <param name="goalId">Goal id</param>
        /// <param name="id">Step id</param>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Step with such id doesn't exist")]
        public async Task<IActionResult> Delete(Guid kidId, Guid pathId, Guid goalId, Guid id)
        {
            await stepService.DeleteAsync(kidId, pathId, goalId, id);

            logger.LogInformation($"Delete step with id: {id}");

            return Ok();
        }
    }
}