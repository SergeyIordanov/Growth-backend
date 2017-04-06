using System;
using System.ComponentModel.DataAnnotations;

namespace Growth.WEB.Models.AccountApiModels
{
    /// <summary>
    /// Role model
    /// </summary>
    public class RoleApiModel
    {
        /// <summary>
        /// Role id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}