namespace TaskPortalApi.Entities
{
    public class TaskEntity : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}