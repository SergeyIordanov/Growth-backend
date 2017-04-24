using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Growth.BLL.DTO.Authorization;
using Growth.BLL.Interfaces;
using Growth.WEB.Models.AccountApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Growth.WEB.Controllers
{
    /// <summary>
    /// Controller that implements account managing operations
    /// </summary>
    [Route("api/")]
    public class AccountController : BaseController
    {
        private readonly IAccountService accountService;
        private readonly IUserService userService;
        private readonly IMapper mapper;
        private readonly ILogger<AccountController> logger;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountService">Instance IAccountService interface that provides operations with user account</param>
        /// <param name="userService">Instance IUserService interface that provides operations with users</param>
        /// <param name="mapper">AutoMapper instance</param>
        /// <param name="logger">Logger</param>
        public AccountController(IAccountService accountService, IUserService userService, IMapper mapper, ILogger<AccountController> logger)
        {
            this.accountService = accountService;
            this.userService = userService;
            this.mapper = mapper;
            this.logger = logger;
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
                var registerModelDto = mapper.Map<RegisterModelDto>(model);
                await accountService.RegisterAsync(registerModelDto);

                logger.LogInformation($"User was registration with email: {model.Email}");

                return Ok();
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Returns info about current user
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Success")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, Description = "Unauthorized")]
        public async Task<IActionResult> Get()
        {
            var userDto = await userService.GetAsync(CurrentUserId);
            var userApiModel = mapper.Map<UserApiModel>(userDto);

            return Json(userApiModel);
        }
    }
}