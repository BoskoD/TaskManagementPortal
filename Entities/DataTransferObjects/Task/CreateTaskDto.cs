using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.Task
{
    public class CreateTaskDto
    {
        [Required]
        public string Project { get; set; }
        public string Id { get; internal set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
}
