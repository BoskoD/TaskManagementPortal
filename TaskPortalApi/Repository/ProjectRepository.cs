using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskPortalApi.Interfaces;
using TaskPortalApi.Models;

namespace TaskPortalApi.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly CloudTable mytable = null;

        public ProjectRepository(IConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("StorageConnectionString"));
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            mytable = cloudTableClient.GetTableReference("Project");
            mytable.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<ProjectEntity>> GetAllAsync()
        {
            return await Task.Run(() => mytable.ExecuteQuery(new TableQuery<ProjectEntity>()));
        }

        public async Task CreateAsync(ProjectEntity myTableOperation)
        {
            await mytable.ExecuteAsync(TableOperation.Insert(myTableOperation));
        }

        public async Task UpdateAsync(ProjectEntity myTableOperation)
        {
            await mytable.ExecuteAsync(TableOperation.InsertOrMerge(myTableOperation));
        }

        public async Task DeleteAsync(ProjectEntity myTableOperation)
        {
            await Task.Run(() => mytable.ExecuteAsync(TableOperation.Delete(myTableOperation)));
        }
        
        public async Task<ProjectEntity> GetAsync(string partitionKey, string RowId)
        {
            var operation = TableOperation.Retrieve<ProjectEntity>(partitionKey, RowId);
            var result = await Task.Run(() => mytable.ExecuteAsync(operation));
            return result.Result as ProjectEntity;
        }
    }
}

