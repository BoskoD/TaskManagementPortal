using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskPortalApi.Entities;
using TaskPortalApi.Interfaces;

namespace TaskPortalApi.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly CloudTable _myTable;

        public TaskRepository(IConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("StorageConnectionString"));
            //var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            _myTable = cloudTableClient.GetTableReference("Task");
            _myTable.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await Task.Run(() => _myTable.ExecuteQuery(new TableQuery<TaskEntity>()));
        }

        public async Task CreateAsync(TaskEntity myTableOperation)
        {
            await Task.Run(() => _myTable.ExecuteAsync(TableOperation.InsertOrReplace(myTableOperation)));
        }

        public async Task UpdateAsync(TaskEntity myTableOperation)
        {
            await _myTable.ExecuteAsync(TableOperation.Replace(myTableOperation));
        }

        public async Task DeleteAsync(TaskEntity myTableOperation)
        {
            await Task.Run(() => _myTable.ExecuteAsync(TableOperation.Delete(myTableOperation)));
        }
       
    }
}