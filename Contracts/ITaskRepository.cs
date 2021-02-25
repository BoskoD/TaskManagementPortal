using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ITaskRepository 
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync();
        Task CreateAsync(TaskEntity myTableOperation);
        Task UpdateAsync(TaskEntity myTableOperation);
        Task DeleteAsync(TaskEntity myTableOperation);
    }
}
