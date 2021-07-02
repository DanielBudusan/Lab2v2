using Lab2v2.Services;
using Lab2v2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2v2.Models
{
    public interface ITaskService
    {

        ActionResult<IEnumerable<TaskViewModel>> FilterTasksByStatus(string status);
        ActionResult<IEnumerable<TaskViewModel>> FilterTasksByDeadline(string startDateTime, string endDateTime);
        Task<IEnumerable<TaskViewModel>> GetTasks();
        ActionResult<IEnumerable<TaskWithCommentsViewModel>> GetComments(int id);
        Boolean PostComment(Comment comment);
        Task<ActionResult<TaskViewModel>> GetTask(int id);
        Task<Boolean> PutTask(int id, Task task);
        Task<ActionResult<Task>> PostTask(TaskViewModel taskViewModel);
        Task<Boolean> DeleteTask(int id);



    }
}
