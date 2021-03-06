namespace TaskManagementPortal.Entities.DataTransferObjects.Project
{
    public class CreateUpdateProjectDto : BaseDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
