using Microsoft.Azure.Cosmos.Table;

namespace TaskManagementPortal.Entities
{
    public class Entity : TableEntity
    {
        public bool Deleted { get; set; } = false;

        public Entity(string partitionKey, string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }

        public Entity()
        {

        }

        public bool IsDeleted()
        {
            return Deleted;
        }
    }
}
