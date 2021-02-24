using System;

namespace TaskPortalApi.DTO.Project
{
    public class DeleteProjectDto
    {
        public string Name { get; set; }
        public string Timestamp { get; } = DateTime.Now.ToString();

    }
}
