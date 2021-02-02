using Microsoft.Azure.Cosmos.Table;

namespace TaskPortalApi.Entities
{
    public class ProjectEntity : TableEntity
    {
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
