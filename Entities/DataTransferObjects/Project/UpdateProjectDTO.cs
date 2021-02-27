namespace Entities.DataTransferObjects.Project
{
    public class UpdateProjectDto : BaseDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
