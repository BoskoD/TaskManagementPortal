namespace TaskManagementPortal.Entities.DataTransferObjects.Task
{
    public class TaskDto : BaseDto
    {
        public string Id { get; set; }
        public string IsComplete { get; set; }
        public string Description { get; set; }
        public string ProjectId { get; set; }
    }
}
