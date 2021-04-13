using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.Contracts;
using Hangfire;
using System;

namespace TaskManagementPortal.TaskPortalApi.APIs.V1.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/")]
    public class NotificationController : Controller
    {
        private readonly ILoggerManger _logger;
        private readonly INotificationRepository _notification;

        public NotificationController(ILoggerManger logger, 
            INotificationRepository notification)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notification = notification ?? throw new ArgumentNullException(nameof(notification));
        }

        [HttpPost]
        [Authorize(Roles = RoleEntity.Admin)]
        [Route("notifications")]
        public IActionResult Notifications(string email)
        {
            RecurringJob.AddOrUpdate(() => _notification.SendEmailNotification(email), Cron.Monthly);
            _logger.LogInfo("Sending emails scheduled!");
            return Ok($"Recurring Job Scheduled. Invoice will be mailed Monthly for {email}!");
        }
    }
}
