using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using TaskManagementPortal.Entities.DataTransferObjects.Project;
using TaskManagementPortal.Entities.DataTransferObjects.Task;
using TaskManagementPortal.Entities.Entities;

namespace TaskManagementPortal.TaskPortalApi.Helpers
{
    public static class ExtensionMethods
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
        }

        public static IEnumerable<UserEntity> WithoutPasswords(this IEnumerable<UserEntity> users) 
        {
            if (users == null) return null;

            return users.Select(x => x.WithoutPassword());
        }

        public static UserEntity WithoutPassword(this UserEntity user) 
        {
            if (user == null) return null;
            user.Password = null;
            return user;
        }
    }
}