using Contracts;
using Entities.Entities;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace TaskPortalApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoggerManger _logger;

        public UserController(IUserRepository userRepository, ILoggerManger logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
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
        [HttpGet]
        public IActionResult GetAll()
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

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
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