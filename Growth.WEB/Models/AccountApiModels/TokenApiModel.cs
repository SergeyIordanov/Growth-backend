namespace Growth.WEB.Models.AccountApiModels
{
    /// <summary>
    /// JWT token model
    /// </summary>
    public class TokenApiModel
    {
        /// <summary>
        /// Token value
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Expiration time (in seconds)
        /// </summary>
        public long ExpiresIn { get; set; }
    }
}