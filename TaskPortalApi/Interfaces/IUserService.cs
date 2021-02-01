using System.Collections.Generic;
using TaskPortalApi.Entities;

namespace TaskPortalApi.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }
}
