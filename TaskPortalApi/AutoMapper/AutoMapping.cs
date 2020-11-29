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
            CreateMap<ProjectEntity, CreateProjectDto>();
            CreateMap<ProjectEntity, UpdateProjectDto>();
            CreateMap<ProjectEntity, DeleteProjectDto>();
            CreateMap<TaskEntity, CreateTaskDto>();
            CreateMap<TaskEntity, UpdateTaskDto>();
            CreateMap<TaskEntity, DeleteTaskDto>();
            CreateMap<TaskEntity, ReadProjectNamesDto>();
        }
    }
}