using System.Collections.Generic;
using TaskManagementPortal.Entities.Entities;

namespace TaskManagementPortal.Contracts
{
    public interface IUserRepository
    {
        UserEntity Authenticate(string username, string password);
        IEnumerable<UserEntity> GetAll();
        UserEntity GetById(int id);
    }
}
