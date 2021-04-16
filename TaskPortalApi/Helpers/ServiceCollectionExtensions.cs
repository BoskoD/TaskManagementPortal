using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.LoggerService;
using TaskManagementPortal.TaskPortalApi.Infrastructure;
using TaskManagementPortal.TaskPortalApi.Repository;

namespace TaskManagementPortal.TaskPortalApi.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAndConfigureApiVersioning(this IServiceCollection services) 
        {
            // configure API versioning
            services.AddApiVersioning(options =>
            {
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
        public static IServiceCollection AddAndConfigureSwagger(this IServiceCollection services) 
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            // configure Swagger
            services.AddSwaggerGen(c =>
            {
                // add a custom operation filter which sets default values
                c.OperationFilter<SwaggerDefaultValues>();

                //// Set the comments path for the Swagger JSON and UI.
                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //c.IncludeXmlComments(xmlPath);

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Authentication",
                    Description = "Enter JWT Bearer token **_only_**",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer", // must be lower case
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {securityScheme, Array.Empty<string>()}
                });
            });

            return services;
        }
        public static IServiceCollection AddAndConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            return services;
        }
        public static IServiceCollection AddAndConfigureOpenTelemetry(this IServiceCollection services)
        {
            services.AddOpenTelemetryTracing(
                (builder) => builder
                   .SetResourceBuilder(
                        ResourceBuilder.CreateDefault().AddService("TaskPortal"))
                            .AddAspNetCoreInstrumentation()
                            .AddConsoleExporter());

            return services;
        }
        public static IServiceCollection AddAndConfigureHealthChecks(this IServiceCollection services) 
        {
            services.AddHealthChecks();
            return services;
        }
        public static IServiceCollection AddAndConfigureDI(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddTransient<IProjectRepository, ProjectRepository>();
            services.AddTransient<ITaskRepository, TaskRepository>();
            services.AddTransient<INotificationRepository, NotificationRepository>();
            services.AddSingleton<ILoggerManger, LoggerManager>();

            return services;
        }
       
    }
}
