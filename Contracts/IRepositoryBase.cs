using Entities.Pagination;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementPortal.Entities.Pagination;

namespace TaskManagementPortal.Contracts
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> ReadAllASync(PaginationProperties paginationProperties);
        Task<IEnumerable<T>> GetAll();
        Task CreateAsync(T myTableOperation);
        Task UpdateAsync(T myTableOperation);
        Task DeleteAsync(T myTableOperation);

    }
}
