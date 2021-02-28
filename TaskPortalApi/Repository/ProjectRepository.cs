using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.Entities.Entities;

namespace TaskManagementPortal.TaskPortalApi.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly CloudTable _myTable;

        public ProjectRepository(IConfiguration configuration)
        {
            var storageAccount = CloudStorageAccount.Parse(configuration.GetConnectionString("StorageConnectionString"));
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            _myTable = cloudTableClient.GetTableReference("Project");
            _myTable.CreateIfNotExistsAsync();
        }

        public async Task<IEnumerable<ProjectEntity>> ReadAllASync()
        {
            return await Task.Run(() => _myTable.ExecuteQuery(new TableQuery<ProjectEntity>()));
        }
        public async Task CreateAsync(ProjectEntity myTableOperation)
        {
            await _myTable.ExecuteAsync(TableOperation.Insert(myTableOperation));
        }
        public async Task UpdateAsync(ProjectEntity myTableOperation)
        {
            await _myTable.ExecuteAsync(TableOperation.InsertOrMerge(myTableOperation));
        }
        public async Task DeleteAsync(ProjectEntity myTableOperation)
        {
            await Task.Run(() => _myTable.ExecuteAsync(TableOperation.Delete(myTableOperation)));
        }
    }
}

