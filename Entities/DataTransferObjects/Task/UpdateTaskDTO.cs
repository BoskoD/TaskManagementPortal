namespace TaskManagementPortal.Entities.DataTransferObjects.Task
{
    public class UpdateTaskDto : BaseDto
    {
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}
