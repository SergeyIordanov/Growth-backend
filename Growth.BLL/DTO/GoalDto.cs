using System;

namespace Growth.BLL.DTO
{
    public class GoalDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool Completed { get; set; }

        public int GoalYear { get; set; }

        public string GoalMonth { get; set; }
    }
}