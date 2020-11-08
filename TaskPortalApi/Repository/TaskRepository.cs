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
        private readonly CloudTable mytable = null;

        public TaskRepository(IConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("StorageConnectionString"));
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            mytable = cloudTableClient.GetTableReference("Task");
            mytable.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await Task.Run(() => mytable.ExecuteQuery(new TableQuery<TaskEntity>()));
        }

        public async Task CreateAsync(TaskEntity myTableOperation)
        {
            await Task.Run(() => mytable.ExecuteAsync(TableOperation.InsertOrReplace(myTableOperation)));
        }

        public async Task UpdateAsync(TaskEntity myTableOperation)
        {
            await mytable.ExecuteAsync(TableOperation.Replace(myTableOperation));
        }

        public async Task DeleteAsync(TaskEntity myTableOperation)
        {
            await Task.Run(() => mytable.ExecuteAsync(TableOperation.Delete(myTableOperation)));
        }

        public async Task<TaskEntity> GetAsync(string partitionKey, string RowId)
        {
            var operation = TableOperation.Retrieve<TaskEntity>(partitionKey, RowId);
            var result = await Task.Run(() => mytable.ExecuteAsync(operation));
            return result.Result as TaskEntity;
        }
    }
}