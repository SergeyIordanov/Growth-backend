using System;
using System.Collections.Generic;

namespace Growth.WEB.Models.AccountApiModels
{
    /// <summary>
    /// User model
    /// </summary>
    public class UserApiModel
    {
        /// <summary>
        /// User id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// User name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Roles of this user
        /// </summary>
        public IEnumerable<string> Roles { get; set; }
    }
}