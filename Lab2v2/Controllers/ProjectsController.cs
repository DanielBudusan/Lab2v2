using AutoMapper;
using Lab2v2.Data;
using Lab2v2.Models;
using Lab2v2.Services;
using Lab2v2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Lab2v2.Controllers
{
    [Authorize(AuthenticationSchemes = "Identity.Application,Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private IProjectService _projectService;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectsController(IProjectService projectService, UserManager<ApplicationUser> userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<ActionResult> AddProject(Project project)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            if (user == null)
            {
                return NotFound();
            }

          
            await _projectService.AddProject(user, project);

            return Ok();

        }

        [HttpGet]
        public async Task<IEnumerable<ProjectViewModel>> GetProjects()
        {
            return await _projectService.GetProjects();
            
        }

        [HttpPut("PutUserToProject/{id}")]
        public async Task<ActionResult> PutUserToProject(int id)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (user == null)
            {
                return NotFound();
            }
            await _projectService.PutUserToProject(user, id);
            return Ok();
        }

        [HttpPut("UpdateProject")]
        public async Task<ActionResult> UpdateProject(UpdateProjectViewModel projectToUpdate)
        {
            var response = await _projectService.UpdateProject(projectToUpdate);
            if (response == false)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var response = await _projectService.DeleteProject(id);
            if (response == false)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }

        }

       
    }
}
