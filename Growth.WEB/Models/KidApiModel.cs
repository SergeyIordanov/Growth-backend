using System;
using System.ComponentModel.DataAnnotations;

namespace Growth.WEB.Models
{
    /// <summary>
    /// Kid model
    /// </summary>
    public class KidApiModel
    {
        /// <summary>
        /// Kid's id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Kid name
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Kid's gender (Male or Female)
        /// </summary>
        [Required]
        public string Gender { get; set; }

        /// <summary>
        /// Kid's photo represented as byte array
        /// </summary>
        public string Photo { get; set; }
    }
}