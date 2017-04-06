using System;
using Microsoft.IdentityModel.Tokens;

namespace Growth.WEB.Authentication
{
    /// <summary>
    /// Options for configuring JWT tokens providing
    /// </summary>
    public class TokenProviderOptions
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TokenProviderOptions()
        {
            Path = "/api/token";
            Expiration = TimeSpan.FromHours(2);
        }

        /// <summary>
        /// Secret key for symmetric encription
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Route for receiving a JWT token
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Tokens' provider name
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// The audience for receiving JWT token
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Time of token's expiration
        /// </summary>
        public TimeSpan Expiration { get; set; }

        /// <summary>
        /// Signing credentials
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }
    }
}