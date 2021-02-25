using Entities.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectEntity>> GetAllAsync();
        Task CreateAsync(ProjectEntity myTableOperation);
        Task UpdateAsync(ProjectEntity myTableOperation);
        Task DeleteAsync(ProjectEntity myTableOperation);
    }
}
