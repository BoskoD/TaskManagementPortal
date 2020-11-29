using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace TaskPortalApi.Services
{
    public interface IUserService
    {
        bool IsAnExistingUser(string userName);
        bool IsValidUserCredentials(string userName, string password);
        string GetUserRole(string userName);
    }

    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        private readonly IDictionary<string, string> _users = new Dictionary<string, string>
        {
            { "v-vlto", "qwerty" },
            { "v-bodani", "qwerty" },
            { "admin", "P@ssw0rd" }
        };

        // inject your database here for user validation
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public bool IsValidUserCredentials(string userName, string password)
        {
            _logger.LogInformation($"Validating user [{userName}]");
            if (string.IsNullOrWhiteSpace(userName))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                return false;
            }
            return _users.TryGetValue(userName, out var p) && p == password;
        }

        public bool IsAnExistingUser(string userName)
        {
            return _users.ContainsKey(userName);
        }

        public string GetUserRole(string userName)
        {
            if (!IsAnExistingUser(userName))
            {
                return string.Empty;
            }

            if (userName == "admin")
            {
                return UserRoles.Admin;
            }
            return UserRoles.BasicUser;
        }
    }
}