using System;
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
        private readonly IProjectRepository projectRepository;
        private readonly ITaskRepository taskRepository;
        private readonly ILogger<ProjectController> logger;

        public ProjectController(IProjectRepository projectRepository, ITaskRepository taskRepository, ILogger<ProjectController> logger)
        {
            this.projectRepository = projectRepository;
            this.taskRepository = taskRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Creates new Project.
        /// </summary>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateProjectDTO projectModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }
            try
            {
                logger.LogInformation("Populating new entity...");
                await projectRepository.CreateAsync(new ProjectEntity
                {
                    PartitionKey = projectModel.Name,
                    RowKey = Guid.NewGuid().ToString(),
                    Description = projectModel.Description,
                    Code = projectModel.Code
                });
                logger.LogInformation("Task completed.");
            }
            catch (Exception e)
            {
                logger.LogError(e.Message);
            }
            finally
            {
                logger.LogInformation("Operation finished.");
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
            logger.LogInformation("Pulling entities from repository");
            var entities = await Task.Run(() => projectRepository.GetAllAsync());

            if (entities == null)
            {
                logger.LogWarning("No records found.");
            }

            var model = entities.Select(x => new CreateProjectDTO
            {
                Name = x.PartitionKey,
                Id = x.RowKey,
                Description = x.Description,
                Code = x.Code
            });
            logger.LogInformation("Print all table records.");
            return Ok(model);
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
            logger.LogInformation("Pulling entities from repository...");
            var entities = await Task.Run(() => projectRepository.GetAllAsync());
            ProjectEntity projectEntity;
            try
            {
                logger.LogInformation("Searching for the specified project...");
                projectEntity = entities.First(e => e.RowKey == id);
            }
            catch (Exception e)
            {
                logger.LogCritical(e.Message);
                return NotFound();
            }
            finally
            {
                logger.LogInformation("Operation finished.");
            }
            return Ok(projectEntity);
        }

        /// <summary>
        /// Update the project
        /// </summary>
        /// <param name="projectModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] UpdateProjectDTO projectModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            logger.LogInformation("Re-Populating existing record...");
            await projectRepository.UpdateAsync(new ProjectEntity
            {
                // client should not change this partK & rowK
                RowKey = projectModel.Id,
                PartitionKey = projectModel.Name,
                Description = projectModel.Description,
                Code = projectModel.Code,
                ETag = "*"
            });
            logger.LogInformation("Task completed");
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
                logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            try
            {
                var projectEntity = projectRepository.GetAllAsync().Result.FirstOrDefault(p => p.RowKey == id);
                await projectRepository.DeleteAsync(projectEntity);
            }
            catch (Exception e)
            {
                logger.LogCritical(e.Message);
            }
            return Ok();
        }

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="id"></param>
        /// <param name="projectModel"></param>
        /// <returns>Object id that has been removed</returns>
        [HttpDelete("deleteobject")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProject(string id, [FromBody] DeleteProjectDTO projectModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            logger.LogInformation("Populating record before deletion...");
            await projectRepository.DeleteAsync(new ProjectEntity
            {
                PartitionKey = projectModel.Name,
                RowKey = id,
                ETag = "*"
            });
            logger.LogInformation("Deleted record from repository.");
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
            logger.LogInformation("Pulling entities from repository...");

            var entities = await Task.Run(() => 
                taskRepository.GetAllAsync().Result.Where(t => t.PartitionKey.Contains(id)));

            logger.LogInformation($"Entities found {entities.Count()}");

            if (entities.Count() == 0)
            {
                logger.LogWarning("Not Found");
            }
            return Ok(entities);
        }
    }
}

