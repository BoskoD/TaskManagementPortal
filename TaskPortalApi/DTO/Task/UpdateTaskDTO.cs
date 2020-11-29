namespace TaskPortalApi.DTO.Task
{
    public class UpdateTaskDto
    {
        public string Project { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }

    }
}
