using System;

namespace Growth.BLL.DTO
{
    public class StepDto
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public bool Completed { get; set; }
    }
}