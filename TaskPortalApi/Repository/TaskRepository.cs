using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskPortalApi.Interfaces;
using TaskPortalApi.Models;

namespace TaskPortalApi.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly CloudTable myTable = null;

        public TaskRepository(IConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("StorageConnectionString"));
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            myTable = cloudTableClient.GetTableReference("Task");
            myTable.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await Task.Run(() => myTable.ExecuteQuery(new TableQuery<TaskEntity>()));
        }

        public async Task CreateAsync(TaskEntity myTableOperation)
        {
            await Task.Run(() => myTable.ExecuteAsync(TableOperation.InsertOrReplace(myTableOperation)));
        }

        public async Task UpdateAsync(TaskEntity myTableOperation)
        {
            await myTable.ExecuteAsync(TableOperation.Replace(myTableOperation));
        }

        public async Task DeleteAsync(TaskEntity myTableOperation)
        {
            await Task.Run(() => myTable.ExecuteAsync(TableOperation.Delete(myTableOperation)));
        }
       
    }
}