using System;
using System.ComponentModel.DataAnnotations;

namespace Growth.WEB.Models
{
    /// <summary>
    /// Path model
    /// </summary>
    public class PathApiModel
    {
        /// <summary>
        /// Path id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Path title
        /// </summary>
        [Required]
        public string Title { get; set; }

        /// <summary>
        /// Path description
        /// </summary>
        [Required]
        public string Description { get; set; }

    }
}