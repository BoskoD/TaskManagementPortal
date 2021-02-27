using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Contracts;
using Entities.DataTransferObjects.Project;
using Entities.Entities;

namespace TaskPortalApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILoggerManger _logger;

        public ProjectController(ILoggerManger logger, 
            IProjectRepository projectRepository, 
            ITaskRepository taskRepository, 
            IMemoryCache memoryCache)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto projectDto)
        {
            try
            {
                if (projectDto == null)
                {
                    _logger.LogError("Object sent from client is null.");
                    return BadRequest("Owner object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await _projectRepository.CreateAsync(new ProjectEntity
                {
                    PartitionKey = projectDto.Name,
                    RowKey = Guid.NewGuid().ToString(),
                    Description = projectDto.Description,
                    Code = projectDto.Code,
                });
                return Ok(projectDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside project Create action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("readall")]
        public async Task<IActionResult> ReadAll()
        {
            try
            {
                _logger.LogInfo($"Returned all projects from database.");
                if (!_memoryCache.TryGetValue("Entities", out IEnumerable<ProjectEntity> entities))
                {
                    _memoryCache.Set("Entities", await _projectRepository.GetAllAsync());
                }
                entities = _memoryCache.Get("Entities") as IEnumerable<ProjectEntity>;

                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ReadAll action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }

        }

        [HttpGet("readbyid/{id}")]
        public async Task<IActionResult> ReadById(string id)
        {
            try
            {
                var entities = await _projectRepository.GetAllAsync();
                ProjectEntity projectEntity;
                projectEntity = entities.FirstOrDefault(e => e.RowKey == id);
                if (projectEntity == null)
                {
                    _logger.LogError($"Project with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with id: {id}");
                    return Ok(projectEntity);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ReadById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProjectDto updateProjectDto)
        {
            try
            {
                if (updateProjectDto == null)
                {
                    _logger.LogError("Object sent from client is null.");
                    return BadRequest("Object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid object sent from client.");
                    return BadRequest("Invalid model object");
                }
                await _projectRepository.UpdateAsync(new ProjectEntity
                {
                    RowKey = id,
                    PartitionKey = updateProjectDto.Name,
                    Description = updateProjectDto.Description,
                    Code = updateProjectDto.Code,
                    ETag = "*"
                });
                return Ok(updateProjectDto);
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
                var projectEntity = _projectRepository.GetAllAsync().Result.FirstOrDefault(p => p.RowKey == id);
                if (projectEntity == null)
                {
                    _logger.LogError($"Project with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                await _projectRepository.DeleteAsync(projectEntity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside Delete action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("deleteproject")]
        public async Task<IActionResult> DeleteProject(string id, [FromBody] DeleteProjectDto projectModel)
        {
            try
            {
                await _projectRepository.DeleteAsync(new ProjectEntity
                {
                    PartitionKey = projectModel.Name,
                    RowKey = id,
                    ETag = "*"
                });
                return Ok(projectModel);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteProject action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("readtasksfromproject/{id}")]
        public async Task<ActionResult<TaskEntity>> TasksByProject(string id)
        {
            try
            {
                var entities = await _taskRepository.GetAllAsync();
                if (entities == null)
                {
                    _logger.LogError("Object sent from client is null.");
                    return BadRequest("Object is null");
                }
                IEnumerable<TaskEntity> taskEntities = entities.ToList();
                _logger.LogInfo($"Entities found {taskEntities.Count()}");
                var tasks = taskEntities.Where(t => t.PartitionKey.Contains(id));
                if (!tasks.Any())
                {
                    _logger.LogInfo("Object not found");
                }
                return Ok(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteProject action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

