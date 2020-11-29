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
        private readonly CloudTable myTable = null;

        public ProjectRepository(IConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("StorageConnectionString"));
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            myTable = cloudTableClient.GetTableReference("Project");
            myTable.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<ProjectEntity>> GetAllAsync()
        {
            return await Task.Run(() => myTable.ExecuteQuery(new TableQuery<ProjectEntity>()));
        }

        public async Task CreateAsync(ProjectEntity myTableOperation)
        {
            await myTable.ExecuteAsync(TableOperation.Insert(myTableOperation));
        }

        public async Task UpdateAsync(ProjectEntity myTableOperation)
        {
            await myTable.ExecuteAsync(TableOperation.InsertOrMerge(myTableOperation));
        }

        public async Task DeleteAsync(ProjectEntity myTableOperation)
        {
            await Task.Run(() => myTable.ExecuteAsync(TableOperation.Delete(myTableOperation)));
        }
    }
}

