using System;

namespace Growth.BLL.DTO
{
    public class KidDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Gender { get; set; }

        public byte[] Photo { get; set; }
    }
}