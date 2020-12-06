using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using TaskPortalApi.Interfaces;
using TaskPortalApi.DTO.Project;
using TaskPortalApi.Models;

namespace TaskPortalApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITaskRepository _taskRepository;
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(IProjectRepository projectRepository, ITaskRepository taskRepository, ILogger<ProjectController> logger)
        {
            _projectRepository = projectRepository;
            _taskRepository = taskRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates new Project.
        /// </summary>
        /// <param name="projectDto"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProjectDto projectDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }
            try
            {
                _logger.LogInformation("Populating new entity...");
                await _projectRepository.CreateAsync(new ProjectEntity
                {
                    PartitionKey = projectDto.Name,
                    RowKey = Guid.NewGuid().ToString(),
                    Description = projectDto.Description,
                    Code = projectDto.Code
                });
                _logger.LogInformation("Task completed.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }
            finally
            {
                _logger.LogInformation("Operation finished.");
            }
            return Ok();
        }


        /// <summary>
        /// Lists all projects
        /// </summary>
        /// <returns></returns>
        [HttpGet("readall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReadAll()
        {
            _logger.LogInformation("Pulling entities from repository");
            var entities = await _projectRepository.GetAllAsync();

            if (entities == null)
            {
                _logger.LogWarning("No records found.");
            }

            _logger.LogInformation("Print all table records.");
            return Ok(entities);
        }

        /// <summary>
        /// Get specific project searching by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("readbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        /// <summary>
        /// Update the project
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateProjectDto projectModel)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Re-Populating existing record...");
            await _projectRepository.UpdateAsync(new ProjectEntity
            {
                RowKey = id,
                PartitionKey = projectModel.Name,
                Description = projectModel.Description,
                Code = projectModel.Code,
                ETag = "*"
            });
            _logger.LogInformation("Task completed");
            return Ok(projectModel);
        }

        /// <summary>
        /// Delete specific project by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            try
            {
                var projectEntity = _projectRepository.GetAllAsync().Result.FirstOrDefault(p => p.RowKey == id);
                await _projectRepository.DeleteAsync(projectEntity);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectModel"></param>
        /// <returns>Object id that has been removed</returns>
        [HttpDelete("deleteproject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProject(string id, [FromBody] DeleteProjectDto projectModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

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

        /// <summary>
        /// List all tasks under specific project.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>All tasks for a specific project</returns>
        [HttpGet("readtasksfromproject/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

