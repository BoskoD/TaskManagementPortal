using Microsoft.Azure.Cosmos.Table;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.Entities.Pagination;
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

        public async Task<IEnumerable<TaskEntity>> GetAll()
        {
            return await Task.Run(() => _myTable.ExecuteQuery(new TableQuery<TaskEntity>()));
        }

        public async Task<IEnumerable<TaskEntity>> ReadAllASync(PaginationProperties paginationProperties)
        {
            return await Task.Run(() => _myTable.ExecuteQuery(new TableQuery<TaskEntity>())
            .OrderBy(on => on.Timestamp)
            .Skip((paginationProperties.PageNumber - 1) * paginationProperties.PageSize)
            .Take(paginationProperties.PageSize)
            .ToList());
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