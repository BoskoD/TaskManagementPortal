using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.Entities.Models;

namespace TaskManagementPortal.TaskPortalApi.APIs.V2.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/")]
    public class UserController2 : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggerManger _logger;

        public UserController2(IUserRepository userRepository, ILoggerManger logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [AllowAnonymous]
        [HttpPost("user/authenticate")]
        public IActionResult Authenticate([FromBody] AuthenticateModel model)
        {
            try
            {
                var user = _userRepository.Authenticate(model.Username, model.Password);
                if (user == null)
                {
                    return BadRequest(new { message = "Username or password is incorrect" });
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Authenticate action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [Authorize(Roles = RoleEntity.Admin)]
        [HttpGet("users")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _userRepository.GetAll();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAll users action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("user/{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                // only allow admins to access other user records
                var currentUserId = int.Parse(User.Identity.Name);
                if (id != currentUserId && !User.IsInRole(RoleEntity.Admin))
                {
                    _logger.LogError($"User {currentUserId} does not have enough privilege!");
                    return Forbid();
                }
                var user = _userRepository.GetById(id);

                if (user == null)
                    return NotFound();

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetById users action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}