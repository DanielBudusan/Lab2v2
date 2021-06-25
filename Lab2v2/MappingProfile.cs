using Lab2v2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lab2v2.Models;

namespace Lab2v2
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Models.Task, TaskViewModel>().ReverseMap(); ;
            CreateMap<Comment, CommentViewModel>().ReverseMap(); ;
            CreateMap<Models.Task, TaskWithCommentsViewModel>().ReverseMap();
            CreateMap<Models.Project, ProjectViewModel>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserViewModel>().ReverseMap();
        }
  
    }
}
