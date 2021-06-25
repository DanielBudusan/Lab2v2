using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2v2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IEnumerable<Project> Projects { get; set; }
    }
}
