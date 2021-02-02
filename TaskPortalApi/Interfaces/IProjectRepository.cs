using System.Collections.Generic;
using System.Threading.Tasks;
using TaskPortalApi.Entities;

namespace TaskPortalApi.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectEntity>> GetAllAsync();
        Task CreateAsync(ProjectEntity myTableOperation);
        Task UpdateAsync(ProjectEntity myTableOperation);
        Task DeleteAsync(ProjectEntity myTableOperation);
    }
}
