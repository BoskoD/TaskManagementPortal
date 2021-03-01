using TaskManagementPortal.Entities.Entities;

namespace TaskManagementPortal.Entities.DataTransferObjects.Task
{
    public class UpdateTaskDto : BaseDto
    {
        public ProjectEntity Project { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}
