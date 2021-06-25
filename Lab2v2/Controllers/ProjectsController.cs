using AutoMapper;
using Lab2v2.Data;
using Lab2v2.Models;
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
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProjectsController(ApplicationDbContext context, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpPost]
        public async Task<ActionResult> AddProject(Project project)
        {
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            project.ProjectUsers = new List<ApplicationUser>();
            project.ProjectUsers.Add(user);
            project.CreatedByUserId = user.Id;
            project.DateAdded = DateTime.Now;

            //List<ApplicationUser> users = new List<ApplicationUser>();
            //users.Add(user);

            //var project = new Project
            //{
            //    Name = projectViewModel.Name,
            //    DateAdded = DateTime.Now,
            //    TaskList = new List<Models.Task>(),
            //    ProjectUsers = users
            //};

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            var projectViewModel = _mapper.Map<ProjectViewModel>(project);

            return CreatedAtAction("GetProject", projectViewModel);
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProjectViewModel>> GetProject()
        {
            return _context.Projects.Include(project => project.ProjectUsers).Include(project => project.TaskList).Select(project => _mapper.Map<ProjectViewModel>(project)).ToList();
            
        }

        [HttpPut("PutUserToProject/{id}")]
        public async Task<ActionResult<ProjectViewModel>> PutUserToProject(int id)
        {
            Project project = _context.Projects.Find(id);
            var user = await _userManager.FindByNameAsync(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            project.ProjectUsers = new List<ApplicationUser>();
            project.ProjectUsers.Add(user);

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var projectViewModel = _mapper.Map<ProjectViewModel>(project);
            return Ok(projectViewModel);
        }

        [HttpPut("UpdateProject")]
        public async Task<ActionResult<ProjectViewModel>> UpdateTask(UpdateProjectViewModel projectToUpdate)
        {
            Project project = _context.Projects.Find(projectToUpdate.Id);
            project.Name = projectToUpdate.Name;
            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var projectViewModel = _mapper.Map<ProjectViewModel>(project);
            return Ok(projectViewModel);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
