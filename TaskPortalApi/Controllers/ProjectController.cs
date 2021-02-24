using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using TaskPortalApi.Interfaces;
using TaskPortalApi.DTO.Project;
using TaskPortalApi.Entities;

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
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectRepository projectRepository, ITaskRepository taskRepository, 
            ILogger<ProjectController> logger, IMemoryCache memoryCache)
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
                _logger.LogInformation("Populating new entity...");
                await _projectRepository.CreateAsync(new ProjectEntity
                {
                    PartitionKey = projectDto.Name,
                    RowKey = Guid.NewGuid().ToString(),
                    Description = projectDto.Description,
                    Code = projectDto.Code,
                });
                _logger.LogInformation("Task completed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return BadRequest();
            }
            finally
            {
                _logger.LogInformation("Operation finished.");
            }
            return Ok(projectDto);
        }

        [HttpGet("readall")]
        public async Task<IActionResult> ReadAll()
        {
            _logger.LogInformation("Pulling entities from repository");
            if (!_memoryCache.TryGetValue("Entities", out IEnumerable<ProjectEntity> entities))
            {
                _memoryCache.Set("Entities", await _projectRepository.GetAllAsync());
            }
            entities = _memoryCache.Get("Entities") as IEnumerable<ProjectEntity>;

            if (entities == null)
            {
                _logger.LogWarning("No records found.");
            }

            _logger.LogInformation("Print all table records.");
            return Ok(entities);
        }

        [HttpGet("readbyid/{id}")]
        public async Task<ActionResult<ProjectEntity>> ReadById(string id)
        {
            _logger.LogInformation("Pulling entities from repository...");
            var entities = await _projectRepository.GetAllAsync();
            ProjectEntity projectEntity;
            try
            {
                _logger.LogInformation("Searching for the specified project...");
                projectEntity = entities.FirstOrDefault(e => e.RowKey == id);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return NotFound();
            }
            return Ok(projectEntity);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProjectDto updateProjectDto)
        {
            if (id == null)
            {
                return NotFound();
            }
            _logger.LogInformation("Re-Populating existing record...");
            await _projectRepository.UpdateAsync(new ProjectEntity
            {
                RowKey = id,
                PartitionKey = updateProjectDto.Name,
                Description = updateProjectDto.Description,
                Code = updateProjectDto.Code,
                ETag = "*"
            });
            _logger.LogInformation("Task completed");
            return Ok(updateProjectDto);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var projectEntity = _projectRepository.GetAllAsync().Result.FirstOrDefault(p => p.RowKey == id);
                await _projectRepository.DeleteAsync(projectEntity);
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return BadRequest("Operation did not complete!");
            }
            finally {
                _logger.LogInformation("Operation completed");
            }
        }

        [HttpDelete("deleteproject")]
        public async Task<IActionResult> DeleteProject(string id, [FromBody] DeleteProjectDto projectModel)
        {
            try
            {
                _logger.LogInformation("Populating record before deletion...");
                await _projectRepository.DeleteAsync(new ProjectEntity
                {
                    PartitionKey = projectModel.Name,
                    RowKey = id,
                    ETag = "*"
                });
                _logger.LogInformation("Deleted record from repository.");
                return Ok(projectModel);
            }
            catch (Exception)
            {
                throw;
            }
            finally 
            {
                _logger.LogInformation("Operation completed!");
            }
        }

        [HttpGet("readtasksfromproject/{id}")]
        public async Task<ActionResult<TaskEntity>> TasksByProject(string id)
        {
            _logger.LogInformation("Pulling entities from repository...");
            var entities = await _taskRepository.GetAllAsync();
            IEnumerable<TaskEntity> taskEntities = entities.ToList();
            _logger.LogInformation($"Entities found {taskEntities.Count()}");

            var tasks = taskEntities.Where(t => t.PartitionKey.Contains(id));

            if (!tasks.Any())
            {
                _logger.LogWarning("Not Found");
            }
            return Ok(tasks);
        }
    }
}

