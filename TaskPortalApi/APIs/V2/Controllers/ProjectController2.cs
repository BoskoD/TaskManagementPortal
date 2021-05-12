using System;
using AutoMapper;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;
using TaskManagementPortal.Contracts;
using TaskManagementPortal.Entities.DataTransferObjects.Project;
using TaskManagementPortal.Entities.Entities;
using TaskManagementPortal.Entities.Pagination;

namespace TaskManagementPortal.TaskPortalApi.APIs.V2.Controllers
{
    // Redis 

    [ApiController]
    [Authorize]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/")]
    public class ProjectController2 : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IProjectRepository _projectRepository;
        private readonly ILoggerManger _logger;
        private readonly IMapper _mapper;

        public ProjectController2 (ILoggerManger logger,
            IProjectRepository projectRepository,
            IMemoryCache memoryCache,
            IMapper mapper)
        {
            _projectRepository = projectRepository ?? throw new ArgumentNullException(nameof(projectRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
      

        [HttpPost("project")]
        public async Task<IActionResult> CreateProject([FromBody] CreateUpdateProjectDto createProjectDto)
        {
            try
            {
                if (createProjectDto == null)
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
                    PartitionKey = createProjectDto.Name,
                    RowKey = Guid.NewGuid().ToString(),
                    Description = createProjectDto.Description,
                    Code = createProjectDto.Code,
                });
                return Created("", createProjectDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside project Create action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("projects")]
        public async Task<IActionResult> GetAllProjects([FromQuery] PaginationProperties paginationParameters)
        {
            try
            {
                _logger.LogInfo($"Returned all projects from database.");
                if (!_memoryCache.TryGetValue("Entities", out IEnumerable<ProjectEntity> entities))
                {
                    _memoryCache.Set("Entities", await _projectRepository.ReadAllASync(paginationParameters));
                }
                entities = _memoryCache.Get("Entities") as IEnumerable<ProjectEntity>;
                

                ProjectDto presentation = null;
                List<ProjectDto> projectDtos = new();

                foreach (var entity in entities)
                {
                    presentation = _mapper.Map<ProjectDto>(entity);
                    projectDtos.Add(presentation);
                }

                return Ok(projectDtos);
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
                var entities = await _projectRepository.GetAll();
                var projectEntity = entities.FirstOrDefault(e => e.RowKey == id);

                ProjectDto presentation = null;
                presentation = _mapper.Map<ProjectDto>(projectEntity);

                if (projectEntity == null || projectEntity.Deleted)
                {
                    _logger.LogError($"Project with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned owner with id: {id}");
                    return Ok(presentation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside ReadById action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut("project/{id}")]
        public async Task<IActionResult> UpdateProject(string id, [FromBody] CreateUpdateProjectDto updateProjectDto)
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
                var projectEntity = _projectRepository.GetAll().Result.FirstOrDefault(p => p.RowKey == id);
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

