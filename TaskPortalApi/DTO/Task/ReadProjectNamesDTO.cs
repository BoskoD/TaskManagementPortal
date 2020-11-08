using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskPortalApi.DTO.Task
{
    public class ReadProjectNamesDTO
    {
        public string Name { get; set; }
        public string Id { get; internal set; }

    }
}
