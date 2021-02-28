using System;

namespace TaskManagementPortal.Entities.DataTransferObjects
{
    public class BaseDto
    {
        public string Name { get; set; }
        public string Timestamp { get; } = DateTime.Now.ToString();
    }
}
