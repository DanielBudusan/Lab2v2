using AutoMapper;
using Lab2v2.Data;
using Lab2v2.Models;
using Lab2v2.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Lab2v2.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
     

        public ProjectService (ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }


        public async Task<Boolean> AddProject(ApplicationUser user, Project project)
        {
            //List<ApplicationUser> users = new List<ApplicationUser>();
            //users.Add(user);

            //var project = new Project
            //{
            //    Name = projectViewModel.Name,
            //    DateAdded = DateTime.Now,
            //    TaskList = new List<Models.Task>(),
            //    ProjectUsers = users
            //};

            project.ProjectUsers = new List<ApplicationUser>();
            project.ProjectUsers.Add(user);
            project.CreatedByUserId = user.Id;
            project.DateAdded = DateTime.Now;

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            //var projectViewModel = _mapper.Map<ProjectViewModel>(project);

            return true;
        }

        public async Task<bool> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return false;
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<ProjectViewModel>> GetProjects()
        {
            var query = _context.Projects.Include(project => project.ProjectUsers).Include(project => project.TaskList).Select(project => _mapper.Map<ProjectViewModel>(project));
            return await query.ToListAsync();
        }

        public async Task<Boolean> PutUserToProject(ApplicationUser user, int ProjectId)
        {
            Project project = _context.Projects.Find(ProjectId);
            
            project.ProjectUsers = new List<ApplicationUser>();
            project.ProjectUsers.Add(user);

            _context.Entry(project).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(ProjectId))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            //var projectViewModel = _mapper.Map<ProjectViewModel>(project);
            return true;
        }

        public async Task<bool> UpdateProject(UpdateProjectViewModel projectToUpdate)
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
                    return false;
                }
                else
                {
                    throw;
                }
            }
            //var projectViewModel = _mapper.Map<ProjectViewModel>(project);
            return true;
        }

        

        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.Id == id);
        }
    }
}
