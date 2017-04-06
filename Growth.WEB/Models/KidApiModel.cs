using System;

namespace Growth.WEB.Models
{
    /// <summary>
    /// Kid model
    /// </summary>
    public class KidApiModel
    {
        /// <summary>
        /// Kid name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Kid's gender (Male or Female)
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Parent user id
        /// </summary>
        public Guid UserId { get; set; }
    }
}