using System;

namespace Growth.WEB.Models
{
    /// <summary>
    /// Goal model (part of Path)
    /// </summary>
    public class GoalApiModel
    {
        /// <summary>
        /// Goal id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Goal title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Is goal completed?
        /// </summary>
        public bool Completed { get; set; }

        /// <summary>
        /// Year to complete the goal
        /// </summary>
        public int GoalYear { get; set; }

        /// <summary>
        /// Month to complete the goal (prefarable format: JUN, DEC etc)
        /// </summary>
        public string GoalMonth { get; set; }
    }
}