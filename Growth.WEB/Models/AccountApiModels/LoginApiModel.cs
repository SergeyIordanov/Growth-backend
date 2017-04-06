using System.ComponentModel.DataAnnotations;

namespace Growth.WEB.Models.AccountApiModels
{
    /// <summary>
    /// Model for login
    /// </summary>
    public class LoginApiModel
    {
        /// <summary>
        /// Unique user email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}