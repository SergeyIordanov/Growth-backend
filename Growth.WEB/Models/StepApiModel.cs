using System;
using System.ComponentModel.DataAnnotations;

namespace Growth.WEB.Models
{
    /// <summary>
    /// Step model (part of goal)
    /// </summary>
    public class StepApiModel
    {
        /// <summary>
        /// Step id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Step text
        /// </summary>
        [Required]
        public string Text { get; set; }

        /// <summary>
        /// Is step completed?
        /// </summary>
        public bool Completed { get; set; }
    }
}