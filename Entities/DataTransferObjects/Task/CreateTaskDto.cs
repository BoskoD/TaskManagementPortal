﻿using System.ComponentModel.DataAnnotations;

namespace Entities.DataTransferObjects.Task
{
    public class CreateTaskDto : BaseDto
    {
        [Required(ErrorMessage = "ProjectId is required")]
        public string ProjectId { get; set; }
        public string Description { get; set; }
    }
}
