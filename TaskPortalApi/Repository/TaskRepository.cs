using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.TaskPortalApi.Helpers;

namespace TaskManagementPortal.TaskPortalApi.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly CloudTable _myTable;

        public TaskRepository()
        {
            _myTable = Common.CreateTable("Task");
        }

        public async Task<IEnumerable<TaskEntity>> ReadAllASync()
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