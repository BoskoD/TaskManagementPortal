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
        private readonly ITaskRepository _repository;
        private readonly IProjectRepository _projectRepository;
        private readonly ILogger<TaskController> _logger;

        public TaskController(ITaskRepository repository, IProjectRepository projectRepository, ILogger<TaskController> logger)
        {
            _repository = repository;
            _projectRepository = projectRepository;
            this._logger = logger;
        }

        /// <summary>
        /// Creates new task
        /// </summary>
        /// <param name="taskModel"></param>
        /// <returns></returns>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto taskModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request, operation failure.");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Populating new entity...");
                await _repository.CreateAsync(new TaskEntity
                {
                    PartitionKey = taskModel.Project,
                    RowKey = Guid.NewGuid().ToString(),
                    Name = taskModel.Name,
                    Description = taskModel.Description,
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
            var entities = await _repository.GetAllAsync();

            if (entities == null)
            {
                _logger.LogWarning("No records found.");
                return NotFound();
            }

            var model = entities.Select(x => new CreateTaskDto
            {
                Project = x.PartitionKey,
                Id = x.RowKey,
                Name = x.Name,
                Description = x.Description,
                IsComplete = x.IsComplete
            });
            _logger.LogInformation("Print all table records.");
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
            _logger.LogInformation("Pulling entities from repository...");
            var entities = await _repository.GetAllAsync();
            TaskEntity taskEntity;

            try
            {
                _logger.LogInformation("Searching for the specified Task...");
                taskEntity = entities.First(e => e.RowKey == id);
            }
            catch (Exception e)
            {
                _logger.LogCritical(e.Message);
                return NotFound();
            }
            return Ok(taskEntity);
        }

        /// <summary>
        /// Update the Task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateTaskDto"></param>
        /// <returns></returns>
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateTaskDto updateTaskDto)
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
            await _repository.UpdateAsync(new TaskEntity
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
        /// Delete specific Task.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete(string id)
        {
            var taskEntity = _repository.GetAllAsync().Result.FirstOrDefault(p => p.RowKey == id);
            if (taskEntity == null)
            {
                _logger.LogCritical("Task not found");
                return NotFound();
            }
            await _repository.DeleteAsync(taskEntity);
            _logger.LogInformation($"Deleted task {id} from repository.");
            return Ok();
        }

        /// <summary>
        /// Delete a specific task.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="taskModel"></param>
        /// <returns></returns>
        [HttpDelete("deleteobject/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTask(string id, [FromBody] DeleteTaskDto taskModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogCritical("Model state is not valid in this request.");
                return BadRequest(ModelState);
            }
            _logger.LogInformation("Populating record before deletion...");
            await _repository.DeleteAsync(new TaskEntity
            {
                PartitionKey = taskModel.Project,
                RowKey = id,
                ETag = "*"
            });
            _logger.LogInformation("Deleted record from repository.");
            return Ok(taskModel);

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

