using System;

namespace Lab2v2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateTime { get; set; }

        public int TaskId { get; set; }
        public Task Task { get; set; }
    }
}
