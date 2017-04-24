using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using AutoMapper;
using Growth.BLL.DTO.Authorization;
using Growth.BLL.Infrastructure.Exceptions;
using Growth.BLL.Interfaces;
using Growth.WEB.Authentication.Interfaces;
using Growth.WEB.Models.AccountApiModels;

namespace Growth.WEB.Authentication
{
    /// <summary>
    /// Implementation of IIdentityProvider for providing user's identity
    /// </summary>
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="accountService">Instance IAccountService interface that provides operations with user account</param>
        /// <param name="mapper">AutoMapper instance</param>
        public IdentityProvider(IAccountService accountService, IMapper mapper)
        {
            this.accountService = accountService;
            this.mapper = mapper;
        }

        /// <summary>
        /// Provides user's identity by login model
        /// </summary>
        /// <param name="loginApiModel">Contains credentials for signing in</param>
        /// <returns>User identity with claims</returns>
        public ClaimsIdentity GetIdentity(LoginApiModel loginApiModel)
        {
            var loginModelDto = mapper.Map<LoginModelDto>(loginApiModel);
            UserDto userDto;

            try
            {
                userDto = accountService.Login(loginModelDto);
            }
            catch (ServiceException)
            {
                return null;
            }

            var roleClaims = userDto.Roles.Select(role => new Claim("roles", role)).ToList();

            var claims = new ClaimsIdentity(
                new GenericIdentity(userDto.Email, "Token"),
                roleClaims);

            claims.AddClaim(new Claim("userId", userDto.Id.ToString()));

            return claims;
        }
    }
}