using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2v2.Data;
using Lab2v2.Models;
using Task = Lab2v2.Models.Task;
using Lab2v2.ViewModels;
using AutoMapper;

namespace Lab2v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TasksController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Filter tasks by status: open, in progres or closed
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("filter/status")]
        public ActionResult<IEnumerable<TaskViewModel>> FilterTasksByStatus(string status)
        {
            return _context.Tasks.Where(t => t.Status == status).Select(task => _mapper.Map<TaskViewModel>(task)).ToList();
        }



        /// <summary>
        /// Filter tasks by deadline interval
        /// date format: 2021/05/17 19:00:00
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("filter/deadline")]
        public ActionResult<IEnumerable<TaskViewModel>> FilterTasksByDeadline (string startDateTime, string endDateTime )
        {

            DateTime startDate = Convert.ToDateTime(startDateTime);
            DateTime endDate = Convert.ToDateTime(endDateTime);

            return _context.Tasks.Where(task => task.Deadline >= startDate && task.Deadline < endDate).Select(task => _mapper.Map<TaskViewModel>(task)).ToList();
        }


        /// <summary>
        /// Get all tasks
        /// </summary>
        /// <returns></returns>
        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskViewModel>>> GetTasks()
        {
            return await _context.Tasks.Select(task => _mapper.Map<TaskViewModel>(task)).ToListAsync();
        }


        /// <summary>
        /// Get all comments for a task by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Comments")]
        public ActionResult<IEnumerable<TaskWithCommentsViewModel>> GetComments (int id)
        {
            var query = _context.Tasks.Where(task => task.Id == id).Include(task => task.Comments)
                .Select(task => _mapper.Map<TaskWithCommentsViewModel>(task));
            return query.ToList();
           
        }

        /// <summary>
        /// Post a new comment for a task
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost("Comments")]
        public ActionResult<CommentViewModel> PostComment (Comment comment)
        {
            comment.Task = _context.Tasks.Find(comment.TaskId);
           
            if (comment.Task == null)
            {
                return NotFound();
            }
            comment.DateTime = DateTime.Now;
            _context.Comments.Add(comment);
            _context.SaveChanges();

            var CommentViewModel = _mapper.Map<CommentViewModel>(comment);

            return Ok(CommentViewModel);

        }

        /// <summary>
        /// Display task by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskViewModel>> GetTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }
            var taskViewModel = _mapper.Map<TaskViewModel>(task);
            return Ok(taskViewModel);
        }


        /// <summary>
        /// Update task by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        // PUT: api/Tasks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<TaskViewModel>> PutTask(int id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
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
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            var TaskViewModel = _mapper.Map<TaskViewModel>(task);
            return Ok(TaskViewModel);
        }


        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="taskViewModel"></param>
        /// <returns></returns>
        // POST: api/Tasks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(TaskViewModel taskViewModel)
        {
            Task task = _mapper.Map<Task>(taskViewModel);
            task.DateAdded = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

           
            return CreatedAtAction("GetTask", task);
        }


        /// <summary>
        /// Delete a task by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
