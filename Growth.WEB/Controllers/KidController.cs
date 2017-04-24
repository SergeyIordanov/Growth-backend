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
    [Route("api/me/kids")]
    [Authorize]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
    public class KidController : BaseController
    {
        private readonly IKidService kidService;
        private readonly IMapper mapper;
        private readonly ILogger<KidController> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="kidService">Instance IKidService interface that provides operations with kids</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public KidController(IKidService kidService, IMapper mapper, ILogger<KidController> logger)
        {
            this.kidService = kidService;
            this.mapper = mapper;
            this.logger = logger;
        }

        /// <summary>
        /// Returns kid with id
        /// </summary>
        /// <param name="id">Kid id</param>
        [HttpGet("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Kid model")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Kid with such id doesn't exist")]
        public async Task<IActionResult> Get(Guid id)
        {
            var kidDto = await kidService.GetAsync(CurrentUserId, id);
            var kidApiModel = mapper.Map<KidApiModel>(kidDto);

            return Json(kidApiModel);
        }

        /// <summary>
        /// Returns all kids for current user
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "List of all kids of current user")]
        public async Task<IActionResult> Get()
        {
            var kidDtos = await kidService.GetAllAsync(CurrentUserId);
            var kidApiModels = mapper.Map<IEnumerable<KidApiModel>>(kidDtos);

            return Json(kidApiModels);
        }

        /// <summary>
        /// Creates new kid
        /// </summary>
        /// <param name="kidApiModel">Kid model</param>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid model")]
        public async Task<IActionResult> Post([FromBody] KidApiModel kidApiModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var kidDto = mapper.Map<KidDto>(kidApiModel);

            var id = await kidService.CreateAsync(CurrentUserId, kidDto);

            logger.LogInformation($"New kid {kidApiModel.Name} was created. Id: {id}");

            return Ok(id);
        }

        /// <summary>
        /// Deletes kid with id 
        /// </summary>
        /// <param name="id">Kid id</param>
        [HttpDelete("{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, Description = "Kid with such id doesn't exist")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await kidService.DeleteAsync(CurrentUserId, id);

            logger.LogInformation($"Delete kid with id: {id}");

            return Ok();
        }
    }
}