using System;
using System.Collections.Generic;
using System.Text;

namespace Review.Domain.Entities
{
    public class ReviewEntity
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }= DateTime.UtcNow;
    }
}
