using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lab2v2.Data;
using Lab2v2.ViewModels;

namespace Lab2v2.Validators 
{
    public class TaskValidator : AbstractValidator<TaskViewModel>
    {
        public readonly ApplicationDbContext _context;

        public TaskValidator (ApplicationDbContext context)
        {
            _context = context;
            RuleFor(task => task.Title).MinimumLength(5).MaximumLength(20);
            RuleFor(task => task.Description).MinimumLength(10).MaximumLength(200);
            RuleFor(task => task.Deadline).GreaterThan(task => task.DateAdded.AddDays(1));
            
        }
    }
}
