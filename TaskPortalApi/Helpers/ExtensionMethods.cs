using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using System.Collections.Generic;
using System.Linq;
using TaskManagementPortal.Entities.Entities;

namespace TaskManagementPortal.TaskPortalApi.Helpers
{
    public static class ExtensionMethods
    {
        // Cross origin requests
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

        // Open telemetry
        public static void ConfigureOpenTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetryTracing(
                (builder) => builder
                   .SetResourceBuilder(
                        ResourceBuilder.CreateDefault().AddService("TaskPortal"))
                            .AddAspNetCoreInstrumentation()
                            .AddConsoleExporter());
        }

        // Bearer 
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