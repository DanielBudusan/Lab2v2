﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2v2.ViewModels
{
    public class TaskWithCommentsViewModel
    {

        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime Deadline { get; set; }
        public string Importance { get; set; }
        public string Status { get; set; }
        public DateTime ClosedAt { get; set; }
        public IEnumerable<CommentViewModel> Comments { get; set; }
    }
}
