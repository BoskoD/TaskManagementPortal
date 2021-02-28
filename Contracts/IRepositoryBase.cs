using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskManagementPortal.Contracts
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> ReadAllASync();
        Task CreateAsync(T myTableOperation);
        Task UpdateAsync(T myTableOperation);
        Task DeleteAsync(T myTableOperation);
    }
}
