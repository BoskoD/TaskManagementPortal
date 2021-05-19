using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.Contracts;
using Hangfire;
using System;
using TaskPortalApi.FeatureManager;
using Microsoft.FeatureManagement.Mvc;
using Microsoft.FeatureManagement;

namespace TaskManagementPortal.TaskPortalApi.APIs.V2.Controllers
{
    [ApiController]
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/")]
    public class NotificationController : Controller
    {
        private readonly ILoggerManger _logger;
        private readonly INotificationRepository _notification;
        private readonly IFeatureManager _featureManager;


        public NotificationController(ILoggerManger logger, 
            INotificationRepository notification,
            IFeatureManagerSnapshot featureManager)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _notification = notification ?? throw new ArgumentNullException(nameof(notification));
            _featureManager = featureManager ?? throw new ArgumentNullException(nameof(featureManager));
        }

        [FeatureGate(MyFeatureFlags.Notifications)]
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
