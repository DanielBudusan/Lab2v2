using Lab2v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2v2.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public List<TaskViewModel> TaskList { get; set; }
        public List<ApplicationUserViewModel> ProjectUsers { get; set; }

    }
}
