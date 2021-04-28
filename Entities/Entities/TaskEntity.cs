namespace TaskManagementPortal.Entities.Entities
{
    public class TaskEntity : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string IsComplete { get; set; } = "False";
    }
}
