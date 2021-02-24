using System;

namespace TaskPortalApi.DTO.Project
{
    public class CreateProjectDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string Timestamp { get; } = DateTime.Now.ToString();
    }
}
