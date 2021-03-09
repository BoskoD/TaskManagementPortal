using System.ComponentModel.DataAnnotations;

namespace TaskManagementPortal.Entities.DataTransferObjects.Task
{
    public class CreateTaskDto : BaseDto
    {
        [Required(ErrorMessage = "Project id is required")]
        public string ProjectId { get; set; }
        public string Description { get; set; }
    }
}
