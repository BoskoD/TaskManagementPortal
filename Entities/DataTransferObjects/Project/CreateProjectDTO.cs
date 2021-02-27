namespace Entities.DataTransferObjects.Project
{
    public class CreateProjectDto : BaseDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
