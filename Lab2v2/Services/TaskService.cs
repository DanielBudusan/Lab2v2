using AutoMapper;
using Lab2v2.Data;
using Lab2v2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2v2.Models
{
    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TaskService (ApplicationDbContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Boolean> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return false;
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return true;
        }

        public ActionResult<IEnumerable<TaskViewModel>> FilterTasksByDeadline(string startDateTime, string endDateTime)
        {
            DateTime startDate = Convert.ToDateTime(startDateTime);
            DateTime endDate = Convert.ToDateTime(endDateTime);

            return _context.Tasks.Where(task => task.Deadline >= startDate && task.Deadline < endDate).Select(task => _mapper.Map<TaskViewModel>(task)).ToList();
        }

        public ActionResult<IEnumerable<TaskViewModel>> FilterTasksByStatus(string status)
        {
            return _context.Tasks.Where(t => t.Status == status).Select(task => _mapper.Map<TaskViewModel>(task)).ToList();
        }

        public ActionResult<IEnumerable<TaskWithCommentsViewModel>> GetComments(int id)
        {
            var query = _context.Tasks.Where(task => task.Id == id).Include(task => task.Comments)
                .Select(task => _mapper.Map<TaskWithCommentsViewModel>(task));
            return query.ToList();
        }

        public async Task<ActionResult<TaskViewModel>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return null;
            }
            return  _mapper.Map<TaskViewModel>(task);
            
        }

        public async Task<IEnumerable<TaskViewModel>> GetTasks()
        {
            var query = _context.Tasks.Select(task => _mapper.Map<TaskViewModel>(task));
            return await query.ToListAsync();
        }

        public Boolean PostComment(Comment comment)
        {
            comment.Task = _context.Tasks.Find(comment.TaskId);

            if (comment.Task == null)
            {
                return false;
            }
            comment.DateTime = DateTime.Now;
            _context.Comments.Add(comment);
            _context.SaveChanges();

            //var CommentViewModel = _mapper.Map<CommentViewModel>(comment);

            return true;
        }

        public async Task<ActionResult<Task>> PostTask(TaskViewModel taskViewModel)
        {
            Task task = _mapper.Map<Task>(taskViewModel);
            task.DateAdded = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;

        }

        public async Task<Boolean> PutTask(int id, Task task)
        {
            if (id != task.Id)
            {
                return false;
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
            //var TaskViewModel = _mapper.Map<TaskViewModel>(task);
            return true;
        }

       

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
