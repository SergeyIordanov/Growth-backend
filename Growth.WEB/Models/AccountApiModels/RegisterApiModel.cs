using System.ComponentModel.DataAnnotations;

namespace Growth.WEB.Models.AccountApiModels
{
    /// <summary>
    /// Model for registration
    /// </summary>
    public class RegisterApiModel
    {
        /// <summary>
        /// User name
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
        public string Name { get; set; }

        /// <summary>
        /// Unique user email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Password (has to contain at least 1 letter and 1 digit. Required length: 8 - 20)
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"(?!^[0-9]*$)(?!^[a-zA-Z]*$)^([a-zA-Z0-9]{8,20})$", 
            ErrorMessage = "Password has to contain at least 1 letter and 1 digit. Required length: 8 - 20")]
        public string Password { get; set; }

        /// <summary>
        /// Password confirmation
        /// </summary>
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}