﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.Contracts;
using Hangfire;


namespace TaskManagementPortal.TaskPortalApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/")]
    public class NotificationController : Controller
    {
        private readonly ILoggerManger _logger;
        private readonly INotificationRepository _notification;

        public NotificationController(ILoggerManger logger, 
            INotificationRepository notification)
        {
            _logger = logger;
            _notification = notification;
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