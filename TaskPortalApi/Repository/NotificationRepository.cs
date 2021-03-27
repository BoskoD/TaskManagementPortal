using System;
using TaskManagementPortal.Contracts;

namespace TaskManagementPortal.TaskPortalApi.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        public void SendEmailNotification(string email)
        {
            //Logic to Mail the user
            Console.WriteLine($"Test message, need to implement {email}");
        }
    }
}
