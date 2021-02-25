using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task CreateAsync(T myTableOperation);
        Task UpdateAsync(T myTableOperation);
        Task DeleteAsync(T myTableOperation);
    }
}
