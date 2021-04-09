using Microsoft.Extensions.Configuration;

namespace TaskManagementPortal.TaskPortalApi.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }

        public string StorageConnectionString { get; set; }

        public static AppSettings LoadAppSettings()
        {
            IConfigurationRoot configRoot = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
            AppSettings appSettings = configRoot.Get<AppSettings>();
            return appSettings;
        }
    }
}