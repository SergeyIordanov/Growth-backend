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
        private readonly IKidService _kidService;
        private readonly IMapper _mapper;
        private readonly ILogger<KidController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="kidService">Instance IKidService interface that provides operations with kids</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public KidController(IKidService kidService, IMapper mapper, ILogger<KidController> logger)
        {
            _kidService = kidService;
            _mapper = mapper;
            _logger = logger;
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
            var kidDto = await _kidService.GetAsync(CurrentUserId, id);
            var kidApiModel = _mapper.Map<KidApiModel>(kidDto);

            return Json(kidApiModel);
        }

        /// <summary>
        /// Returns all kids for current user
        /// </summary>
        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "List of all kids of current user")]
        public async Task<IActionResult> Get()
        {
            var kidDtos = await _kidService.GetAllAsync(CurrentUserId);
            var kidApiModels = _mapper.Map<IEnumerable<KidApiModel>>(kidDtos);

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

            var kidDto = _mapper.Map<KidDto>(kidApiModel);

            var id = await _kidService.CreateAsync(CurrentUserId, kidDto);

            _logger.LogInformation($"New kid {kidApiModel.Name} was created. Id: {id}");

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
            await _kidService.DeleteAsync(CurrentUserId, id);

            _logger.LogInformation($"Delete kid with id: {id}");

            return Ok();
        }
    }
}