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
        private ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
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
            return _taskService.FilterTasksByStatus(status);
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

            return _taskService.FilterTasksByDeadline(startDateTime, endDateTime);
        }


        /// <summary>
        /// Get all tasks
        /// </summary>
        /// <returns></returns>
        // GET: api/Tasks
        [HttpGet]
        public async Task<IEnumerable<TaskViewModel>> GetTasks()
        {
            return await _taskService.GetTasks();
        }


        /// <summary>
        /// Get all comments for a task by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/Comments")]
        public ActionResult<IEnumerable<TaskWithCommentsViewModel>> GetComments (int id)
        {
            return _taskService.GetComments(id);
           
        }

        /// <summary>
        /// Post a new comment for a task
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        [HttpPost("Comments")]
        public ActionResult PostComment (Comment comment)
        {
            if (_taskService.PostComment(comment) == false)
            {
                return NotFound();
            }


            return Ok();

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
            var taskView = await _taskService.GetTask(id);
            if (taskView == null)
            {
                return NotFound();
            }

            return taskView;
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
        public async Task<ActionResult> PutTask(int id, Task task)
        {
            try
            {
                await _taskService.PutTask(id, task);
            }
            catch
            {
                return NotFound();
            }

            return NoContent();
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

            var task = await _taskService.PostTask(taskViewModel);

           
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
           if (await _taskService.DeleteTask(id) == false)
            {
                return NotFound();
            }
            return NoContent();
        }

        
    }
}
