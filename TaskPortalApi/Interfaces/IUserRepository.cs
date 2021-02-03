using System.Collections.Generic;
using TaskPortalApi.Entities;

namespace TaskPortalApi.Interfaces
{
    public interface IUserRepository
    {
        UserEntity Authenticate(string username, string password);
        IEnumerable<UserEntity> GetAll();
        UserEntity GetById(int id);
    }
}
