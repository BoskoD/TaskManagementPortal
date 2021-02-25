using Entities.Entities;
using System.Collections.Generic;

namespace Contracts
{
    public interface IUserRepository
    {
        UserEntity Authenticate(string username, string password);
        IEnumerable<UserEntity> GetAll();
        UserEntity GetById(int id);
    }
}
