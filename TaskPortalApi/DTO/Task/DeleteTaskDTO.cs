using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPortalApi.DTO.Task
{
    public class TaskDeleteModelDTO
    {
        public string Project { get;  set; }
        public string TaskName { get; set; }
    }
}
