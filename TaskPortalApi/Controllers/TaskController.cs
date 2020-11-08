using System;
using System.Linq;
using TaskPortalApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskPortalApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using TaskPortalApi.DTO.Task;
using Microsoft.AspNetCore.Authorization;

namespace TaskPortalApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository repository;
        private readonly IProjectRepository projectRepository;
        private readonly ILogger<TaskController> logger;

        public TaskController(ITaskRepository repository, IProjectRepository projectRepository, ILogger<TaskController> logger)
        {
            this.repository = repository;
            this.projectRepository = projectRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Creates new task
        /// </summary>
        /// <param name="taskModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTaskDTO taskModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            try
            {
                logger.LogInformation("Populating new entity...");
                await repository.CreateAsync(new TaskEntity
                {
                    PartitionKey = taskModel.Project,
                    RowKey = Guid.NewGuid().ToString(),
                    Name = taskModel.Name,
                    Description = taskModel.Description,
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
        /// List all tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet("readall")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ReadAll()
        {
            logger.LogInformation("Pulling entities from repository");
            var entities = await Task.Run(() => repository.GetAllAsync());

            if (entities == null)
            {
                logger.LogWarning("No records found.");
            }

            var model = entities.Select(x => new CreateTaskDTO
            {
                Project = x.PartitionKey,
                Id = x.RowKey,
                Name = x.Name,
                Description = x.Description,
                IsComplete = x.IsComplete
            });
            logger.LogInformation("Print all table records.");
            return Ok(model);
        }

        /// <summary>
        /// Get specific Task, ID lookup
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("readbyid/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskEntity>> ReadById(string id)
        {
            logger.LogInformation("Pulling entities from repository...");
            var entities = await Task.Run(() => repository.GetAllAsync());
            TaskEntity TaskEntity;

            try
            {
                logger.LogInformation("Searching for the specified Task...");
                TaskEntity = entities.First(e => e.RowKey == id);
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
            return Ok(TaskEntity);
        }

        /// <summary>
        /// Update the Task.
        /// </summary>
        /// <param name="TaskModel"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public async Task<IActionResult> Update([FromBody] TaskUpdateModelDTO TaskModel)
        {

            if (!ModelState.IsValid)
            {
                logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            logger.LogInformation("Re-Populating existing record...");
            await repository.UpdateAsync(new TaskEntity
            {
                // client should not change this partK & rowK
                RowKey = TaskModel.Id,
                PartitionKey = TaskModel.Project,
                Name = TaskModel.Name,
                Description = TaskModel.Description,
                IsComplete = TaskModel.IsComplete,
                ETag = "*"
            });

            logger.LogInformation("Task completed");
            return Ok(TaskModel);
        }

        /// <summary>
        /// Delete specific Task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var taskEntity = repository.GetAllAsync().Result.FirstOrDefault(p => p.RowKey == id);
            if (taskEntity == null)
            {
                logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }
            await repository.DeleteAsync(taskEntity);
            logger.LogInformation("Deleted record from repository.");
            return Ok();
        }

        /// <summary>
        /// Delete a specific task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="TaskModel"></param>
        /// <returns></returns>
        [HttpDelete("deleteobject/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTask(string id, [FromBody] TaskDeleteModelDTO TaskModel)
        {
            if (!ModelState.IsValid)
            {
                logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }
            logger.LogInformation("Populating record before deletion...");
            await repository.DeleteAsync(new TaskEntity
            {
                PartitionKey = TaskModel.Project,
                RowKey = id,
                ETag = "*"
            });
            logger.LogInformation("Deleted record from repository.");
            return Ok(TaskModel);

        }

        /// <summary>
        /// Gets all projects, id and name
        /// </summary>
        /// <returns></returns>
        [HttpGet("readallprojectnames")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> GetAllProjectNames()
        {
            logger.LogInformation("Pulling entities from repository");
            var entities = await Task.Run(() => projectRepository.GetAllAsync());

            if (entities == null)
            {
                logger.LogWarning("No records found.");
            }

            var model = entities.Select(x => new ReadProjectNamesDTO
            {
                Name = x.PartitionKey,
                Id = x.RowKey
            });
            logger.LogInformation("Print all table records.");
            return Ok(model);
        }
    }
}

