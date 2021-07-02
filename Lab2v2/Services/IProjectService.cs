using Lab2v2.Models;
using Lab2v2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2v2.Services
{
    public interface IProjectService
    {
        Task<Boolean> AddProject(ApplicationUser user, Project project);
        Task<IEnumerable<ProjectViewModel>> GetProjects();
        Task<Boolean> PutUserToProject(ApplicationUser user, int ProjectId);
        Task<Boolean> UpdateProject(UpdateProjectViewModel projectToUpdate);
        Task<Boolean> DeleteProject(int id);

    }
}
