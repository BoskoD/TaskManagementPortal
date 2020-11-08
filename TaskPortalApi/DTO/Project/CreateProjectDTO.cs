namespace TaskPortalApi.DTO.Project
{
    public class CreateProjectDTO
    {
        public string Id { get; internal set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
