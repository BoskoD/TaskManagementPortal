using AutoMapper;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.Entities.DataTransferObjects.Project;


namespace TaskManagementPortal.TaskPortalApi.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectEntity, ProjectDto>()
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(src => src.PartitionKey))
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(src => src.RowKey))
                .ForMember(dest =>
                    dest.Code,
                    opt => opt.MapFrom(src => src.Code))
                .ForMember(dest =>
                    dest.Description,
                    opt => opt.MapFrom(src => src.Description))
                .ForAllOtherMembers(m => m.Ignore());
        }
    }
}
