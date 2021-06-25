using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation;
using Lab2v2.Data;
using Lab2v2.ViewModels;
using Lab2v2.Models;


namespace Lab2v2.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public readonly ApplicationDbContext _context;

        public CommentValidator(ApplicationDbContext context)
        {
            
            _context = context;
            RuleFor(comment => comment.Content).Custom((prop, validationContext) =>
            {
                var instance = validationContext.InstanceToValidate;
                string taskStatus = _context.Tasks.Where(task1 => task1.Id == instance.TaskId).Select(task => task.Status).FirstOrDefault();
                
                if (taskStatus == "closed")
                {
                    validationContext.AddFailure("comments not permitted for closed tasks");
                }

            });






        }
    }
}