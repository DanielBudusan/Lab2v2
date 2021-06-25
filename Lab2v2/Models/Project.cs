using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2v2.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateAdded { get; set; }
        public List<Task> TaskList { get; set; }
        public string CreatedByUserId { get; set; }
        public List<ApplicationUser> ProjectUsers { get; set; }

    }
}
