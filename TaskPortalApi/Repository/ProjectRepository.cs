using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.TaskPortalApi.Helpers;


namespace TaskManagementPortal.TaskPortalApi.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly CloudTable _myTable;

        public ProjectRepository()
        {
            _myTable = Common.CreateTable("Project");
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

