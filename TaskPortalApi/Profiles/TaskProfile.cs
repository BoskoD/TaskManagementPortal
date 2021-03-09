using AutoMapper;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.Entities.DataTransferObjects.Task;


namespace TaskManagementPortal.TaskPortalApi.Profiles
{
    public class TaskProfile : Profile
    {
        public TaskProfile()
        {
            CreateMap<TaskEntity, TaskDto>()
               .ForMember(dest =>
                   dest.Name,
                   opt => opt.MapFrom(src => src.Name))
               .ForMember(dest =>
                   dest.Id,
                   opt => opt.MapFrom(src => src.RowKey))
               .ForMember(dest =>
                   dest.ProjectId,
                   opt => opt.MapFrom(src => src.PartitionKey))
               .ForMember(dest =>
                   dest.Description,
                   opt => opt.MapFrom(src => src.Description))
               .ForMember(dest =>
                   dest.IsComplete,
                   opt => opt.MapFrom(src => src.IsComplete))
               .ForAllOtherMembers(m => m.Ignore());
        }
    }
}

