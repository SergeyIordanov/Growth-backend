using System;
using System.Security.Claims;

namespace Growth.WEB.Infrastructure
{
    /// <summary>
    /// Extencions for ClaimsPrincipal
    /// </summary>
    public static class ClaimsPrincipalExtentions
    {
        private const string UserIdClaimName = "userId";

        /// <summary>
        /// Provides id of current user
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static Guid? GetUserId(this ClaimsPrincipal principal)
        {
            try
            {
                var claim = principal.FindFirst(UserIdClaimName);

                if (claim == null)
                {
                    return null;
                }

                var guid = Guid.Parse(claim.Value);

                return guid;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
    }
}