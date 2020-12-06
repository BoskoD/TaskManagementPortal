using System;
using System.Collections.Generic;
using System.Linq;
using TaskPortalApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskPortalApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TaskPortalApi.DTO.Task;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

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
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskRepository taskRepository, IProjectRepository projectRepository, 
            ILogger<TaskController> logger, IMemoryCache memoryCache)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository; 
            _logger = logger;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Create new task
        /// </summary>
        /// <param name="createTaskDto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Populating new entity...");
                await _taskRepository.CreateAsync(new TaskEntity
                {
                    PartitionKey = createTaskDto.Project,
                    RowKey = Guid.NewGuid().ToString(),
                    Name = createTaskDto.Name,
                    Description = createTaskDto.Description,
                });
                _logger.LogInformation("Task completed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            return Ok();
        }

        /// <summary>
        /// List all tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet("readall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReadAll()
        {
            _logger.LogInformation("Pulling entities from repository");
            if (!_memoryCache.TryGetValue("Entities", out IEnumerable<TaskEntity> entities))
            {
                _memoryCache.Set("Entities", await _taskRepository.GetAllAsync());
            }
            entities = _memoryCache.Get("Entities") as IEnumerable<TaskEntity>;

            if (entities == null)
            {
                _logger.LogWarning("No records found.");
            }

            _logger.LogInformation("Print all table records.");
            return Ok(entities);
        }

        /// <summary>
        /// Get specific task searching by rowKey
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("readbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskEntity>> ReadById(string id)
        {
            _logger.LogInformation("Pulling entities from repository...");
            var entities = await _taskRepository.GetAllAsync();
            TaskEntity taskEntity;

            try
            {
                _logger.LogInformation("Searching for the specified Task...");
                taskEntity = entities.FirstOrDefault(e => e.RowKey == id);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return NotFound();
            }
            return Ok(taskEntity);
        }

        /// <summary>
        /// Update the task
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateTaskDto"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Re-Populating existing record...");
            await _taskRepository.UpdateAsync(new TaskEntity
            {
                RowKey = id,
                PartitionKey = updateTaskDto.Project,
                Name = updateTaskDto.Name,
                Description = updateTaskDto.Description,
                IsComplete = updateTaskDto.IsComplete,
                ETag = "*"
            });
            _logger.LogInformation("Task completed");
            return Ok(updateTaskDto);
        }

        /// <summary>
        /// Delete specific task by rowKey.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var taskEntity = _taskRepository.GetAllAsync().Result.FirstOrDefault(p => p.RowKey == id);
            if (taskEntity == null)
            {
                _logger.LogCritical("Task not found");
                return NotFound();
            }
            await _taskRepository.DeleteAsync(taskEntity);
            _logger.LogInformation($"Deleted task {id} from repository.");
            return Ok();
        }

        /// <summary>
        /// Delete a specific task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deleteTaskDto"></param>
        /// <returns></returns>
        [HttpDelete("deletetask/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTask(string id, [FromBody] DeleteTaskDto deleteTaskDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request.");
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Populating record before deletion...");
            await _taskRepository.DeleteAsync(new TaskEntity
            {
                PartitionKey = deleteTaskDto.Project,
                RowKey = id,
                ETag = "*"
            });
            _logger.LogInformation("Deleted record from repository.");
            return Ok(deleteTaskDto);

        }

        /// <summary>
        /// List all projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("readallprojectnames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllProjectNames()
        {
            _logger.LogInformation("Pulling entities from repository");
            var entities = await _projectRepository.GetAllAsync();

            if (entities == null)
            {
                _logger.LogWarning("No records found.");
                return NotFound();
            }

            var model = entities.Select(x => new ReadProjectNamesDto
            {
                Id = x.RowKey,
                Name = x.PartitionKey
            });
            _logger.LogInformation("Print all table records.");
            return Ok(model);
        }
    }
}

