using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Growth.BLL.DTO.Authorization;
using Growth.BLL.Interfaces;
using Growth.WEB.Models.AccountApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Growth.WEB.Controllers
{
    /// <summary>
    /// Controller that implements account managing operations
    /// </summary>
    [Route("api/")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountService">Instance IAccountService interface that provides operations with user account</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public AccountController(IAccountService accountService, IMapper mapper, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="model">Contains user's login, password, name</param>
        [HttpPost("register")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, Description = "Invalid Model")]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError, Description = "Internal server exception")]
        public async Task<IActionResult> Register([FromBody] RegisterApiModel model)
        {
            if (ModelState.IsValid)
            {
                var registerModelDto = _mapper.Map<RegisterModelDto>(model);
                await _accountService.RegisterAsync(registerModelDto);

                _logger.LogInformation($"User was registration with email: {model.Email}");

                return Ok();
            }

            return BadRequest(ModelState);
        }
    }
}