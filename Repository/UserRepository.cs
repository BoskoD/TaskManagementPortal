using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TaskPortalApi.Entities;
using TaskPortalApi.Helpers;
using TaskPortalApi.Interfaces;

namespace TaskPortalApi.Repository
{
    public class UserRepository : IUserRepository
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private readonly List<UserEntity> _users = new List<UserEntity>
        { 
            new UserEntity { Id = 1, FirstName = "Bosko", LastName = "Danilovic", Username = "admin", Password = "admin", Role = RoleEntity.Admin },
            new UserEntity { Id = 2, FirstName = "Vladimir", LastName = "Toshchev", Username = "v-vlto", Password = "qwerty", Role = RoleEntity.User },
            new UserEntity { Id = 3, FirstName = "Bosko", LastName = "Danilovic", Username = "v-bodani", Password = "qwerty", Role = RoleEntity.User }
        };

        private readonly AppSettings _appSettings;

        public UserRepository(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public UserEntity Authenticate(string username, string password)
        {
            var user = _users.SingleOrDefault(x => x.Username == username && x.Password == password);

            // return null if user not found
            if (user == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[] 
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);

            return user.WithoutPassword();
        }

        public IEnumerable<UserEntity> GetAll()
        {
            return _users.WithoutPasswords();
        }

        public UserEntity GetById(int id) 
        {
            var user = _users.FirstOrDefault(x => x.Id == id);
            return user.WithoutPassword();
        }
    }
}