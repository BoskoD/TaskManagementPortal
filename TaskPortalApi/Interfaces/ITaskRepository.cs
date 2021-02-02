using System.Collections.Generic;
using System.Threading.Tasks;
using TaskPortalApi.Entities;

namespace TaskPortalApi.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync();
        Task CreateAsync(TaskEntity myTableOperation);
        Task UpdateAsync(TaskEntity myTableOperation);
        Task DeleteAsync(TaskEntity myTableOperation);
    }
}
