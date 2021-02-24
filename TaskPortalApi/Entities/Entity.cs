using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPortalApi.Entities
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
