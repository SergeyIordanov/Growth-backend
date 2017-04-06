using System.Security.Claims;
using Growth.WEB.Models.AccountApiModels;

namespace Growth.WEB.Authentication.Interfaces
{
    /// <summary>
    /// Interface for providing identity for user (login)
    /// </summary>
    public interface IIdentityProvider
    {
        /// <summary>
        /// Provides user's identity by login model
        /// </summary>
        /// <param name="loginApiModel">Contains credentials for signing in</param>
        /// <returns>User identity with claims</returns>
        ClaimsIdentity GetIdentity(LoginApiModel loginApiModel);
    }
}