namespace TaskManagementPortal.Contracts
{
    public interface INotificationRepository
    {
        void SendEmailNotification(string email);
    }
}
