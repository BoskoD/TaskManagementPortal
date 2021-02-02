using System.Collections.Generic;
using TaskPortalApi.Entities;

namespace TaskPortalApi.Interfaces
{
    public interface IUserService
    {
        UserEntity Authenticate(string username, string password);
        IEnumerable<UserEntity> GetAll();
        UserEntity GetById(int id);
    }
}
