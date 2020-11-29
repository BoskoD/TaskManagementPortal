using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskPortalApi.DTO.Project;
using TaskPortalApi.DTO.Task;
using TaskPortalApi.Models;

namespace TaskPortalApi.AutoMapper
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<ProjectEntity, CreateProjectDTO>();
            CreateMap<ProjectEntity, UpdateProjectDTO>();
            CreateMap<ProjectEntity, DeleteProjectDTO>();
            CreateMap<TaskEntity, CreateTaskDTO>();
            CreateMap<TaskEntity, UpdateTaskDTO>();
            CreateMap<TaskEntity, DeleteTaskDTO>();
            CreateMap<TaskEntity, ReadProjectNamesDTO>();
        }
    }
}