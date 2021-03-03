using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.Entities.DataTransferObjects.Project;
using TaskManagementPortal.Entities.Entities;

namespace TaskManagementPortal.TaskPortalApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/")]
    public class ProjectController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IProjectRepository _projectRepository;
        private readonly ILoggerManger _logger;

        public ProjectController(ILoggerManger logger, 
            IProjectRepository projectRepository, 
            IMemoryCache memoryCache)
        {
            _projectRepository = projectRepository;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpPost("project")]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto projectDto)
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

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects()
        {
            try
            {
                _logger.LogInfo($"Returned all projects from database.");
                if (!_memoryCache.TryGetValue("Entities", out IEnumerable<ProjectEntity> entities))
                {
                    _memoryCache.Set("Entities", await _projectRepository.ReadAllASync());
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

        [HttpGet("project/{id}")]
        public async Task<IActionResult> GetProjectById(string id)
        {
            try
            {
                var entities = await _projectRepository.ReadAllASync();
                ProjectEntity projectEntity;
                projectEntity = entities.FirstOrDefault(e => e.RowKey == id);

                if (projectEntity == null || projectEntity.Deleted)
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

        [HttpPut("project/{id}")]
        public async Task<IActionResult> UpdateProject(string id, [FromBody] UpdateProjectDto updateProjectDto)
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

        [HttpDelete("project/{id}")]
        public async Task<IActionResult> DeleteProject(string id)
        {
            try
            {
                var projectEntity = _projectRepository.ReadAllASync().Result.FirstOrDefault(p => p.RowKey == id);
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
    }
}

