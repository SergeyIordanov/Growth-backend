using System;

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
        public string Title { get; set; }

        /// <summary>
        /// Path description
        /// </summary>
        public string Description { get; set; }

    }
}