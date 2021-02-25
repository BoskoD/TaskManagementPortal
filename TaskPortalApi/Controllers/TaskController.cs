using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Contracts;
using Entities.DataTransferObjects.Task;
using Entities.Entities;

namespace TaskPortalApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly ILoggerManger _logger;

        public TaskController(ILoggerManger logger, ITaskRepository taskRepository, 
            IProjectRepository projectRepository, IMemoryCache memoryCache)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository; 
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto taskDto)
        {
            try
            {
                if (taskDto == null)
                {
                    _logger.LogError("Object sent from client is null.");
                    return BadRequest("Owner object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await _taskRepository.CreateAsync(new TaskEntity
                {
                    PartitionKey = taskDto.Project,
                    RowKey = Guid.NewGuid().ToString(),
                    Name = taskDto.Name,
                    Description = taskDto.Description
                });
                return Ok(taskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside task Create action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("readall")]
        public async Task<IActionResult> ReadAll()
        {
            try
            {
                _logger.LogInfo($"Returned all projects from database.");
                if (!_memoryCache.TryGetValue("Entities", out IEnumerable<TaskEntity> entities))
                {
                    _memoryCache.Set("Entities", await _taskRepository.GetAllAsync());
                }
                entities = _memoryCache.Get("Entities") as IEnumerable<TaskEntity>;

                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ReadAll action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("readbyid/{id}")]
        public async Task<ActionResult<TaskEntity>> ReadById(string id)
        {
            
            try
            {
                var entities = await _taskRepository.GetAllAsync();
                TaskEntity taskEntity;
                taskEntity = entities.FirstOrDefault(e => e.RowKey == id);
                if (taskEntity == null)
                {
                    _logger.LogError($"Task with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned task with id: {id}");
                    return Ok(taskEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ReadById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            try
            {
                if (updateTaskDto == null)
                {
                    _logger.LogError("Object sent from client is null.");
                    return BadRequest("Object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await _taskRepository.UpdateAsync(new TaskEntity
                {
                    RowKey = id,
                    PartitionKey = updateTaskDto.Project,
                    Name = updateTaskDto.Name,
                    Description = updateTaskDto.Description,
                    IsComplete = updateTaskDto.IsComplete,
                    ETag = "*"
                });
                return Ok(updateTaskDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Update action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var taskEntity = _taskRepository.GetAllAsync().Result.FirstOrDefault(p => p.RowKey == id);
                if (taskEntity == null)
                {
                    _logger.LogError($"Task with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _taskRepository.DeleteAsync(taskEntity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Delete action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("readallprojectnames")]
        public async Task<IActionResult> GetAllProjectNames()
        {
            try
            {
                var entities = await _projectRepository.GetAllAsync();
                if (entities == null)
                {
                    _logger.LogError("No records found.");
                    return NotFound();
                }
                var model = entities.Select(x => new ReadProjectNamesDto
                {
                    Id = x.RowKey,
                    Name = x.PartitionKey
                });
                return Ok(model);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllProjectNames action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

