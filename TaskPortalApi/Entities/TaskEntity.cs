using Microsoft.Azure.Cosmos.Table;

namespace TaskPortalApi.Entities
{
    public class TaskEntity : TableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsComplete { get; set; }
    }
}