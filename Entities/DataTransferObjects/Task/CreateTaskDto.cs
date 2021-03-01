using System.ComponentModel.DataAnnotations;
using TaskManagementPortal.Entities.Entities;

namespace TaskManagementPortal.Entities.DataTransferObjects.Task
{
    public class CreateTaskDto : BaseDto
    {
        [Required(ErrorMessage = "Project name is required")]
        public ProjectEntity Project { get; set; }
        public string Description { get; set; }
    }
}
