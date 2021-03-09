using System;
using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.Entities.DataTransferObjects.Task;
using TaskManagementPortal.Entities.Entities;

namespace TaskManagementPortal.TaskPortalApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/")]
    public class TaskController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ITaskRepository _taskRepository;
        private readonly ILoggerManger _logger;
        private readonly IMapper _mapper;

        public TaskController(ILoggerManger logger, 
            ITaskRepository taskRepository, 
            IMemoryCache memoryCache,
            IMapper mapper)
        {
            _taskRepository = taskRepository;
            _logger = logger;
            _memoryCache = memoryCache;
            _mapper = mapper;
        }

        [HttpPost("task")]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto taskDto)
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
                    PartitionKey = taskDto.ProjectId,
                    RowKey = Guid.NewGuid().ToString().Substring(1,7),
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

        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            try
            {
                _logger.LogInfo($"Returned all projects from database.");
                if (!_memoryCache.TryGetValue("Entities", out IEnumerable<TaskEntity> entities))
                {
                    _memoryCache.Set("Entities", await _taskRepository.ReadAllASync());
                }
                entities = _memoryCache.Get("Entities") as IEnumerable<TaskEntity>;

                TaskDto presentation = null;
                List<TaskDto> taskDtos = new List<TaskDto>();

                foreach (var entity in entities)
                {
                    presentation = _mapper.Map<TaskDto>(entity);
                    taskDtos.Add(presentation);
                }

                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ReadAll action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("task/{id}")]
        public async Task<IActionResult> GetTaskById(string id)
        {
            try
            {
                var entities = await _taskRepository.ReadAllASync();
                var taskEntity = entities.FirstOrDefault(e => e.RowKey == id);

                TaskDto presentation = null;
                presentation = _mapper.Map<TaskDto>(taskEntity);

                if (taskEntity == null || taskEntity.Deleted)
                {
                    _logger.LogError($"Task with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned task with id: {id}");
                    return Ok(presentation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ReadById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("task/{id}")]
        public async Task<IActionResult> UpdateTask(string id, [FromBody] UpdateTaskDto updateTaskDto)
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
                    PartitionKey = updateTaskDto.ProjectId,
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

        [HttpDelete("task/{id}")]
        public async Task<IActionResult> DeleteTask(string id)
        {
            try
            {
                var taskEntity = _taskRepository.ReadAllASync().Result.FirstOrDefault(p => p.RowKey == id);
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

        [HttpGet("project/{id}/tasks")]
        public async Task<ActionResult<TaskEntity>> GetTasksByProject(string id)
        {
            try
            {
                var entities = await _taskRepository.ReadAllASync();
                if (entities == null)
                {
                    _logger.LogError("Object sent from client is null.");
                    return BadRequest("Object is null");
                }
                // filter by partitionKey which represents ProjectId
                var tasks = entities.Where(t => t.PartitionKey.Contains(id));

                _logger.LogInfo($"Entities found {tasks.Count()}");

                TaskDto presentation = null;
                List<TaskDto> taskDtos = new List<TaskDto>();

                foreach (var task in tasks)
                {
                    presentation = _mapper.Map<TaskDto>(task);
                    taskDtos.Add(presentation);
                }

                if (!tasks.Any())
                {
                    _logger.LogInfo("Object not found");
                }

                return Ok(taskDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteProject action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

