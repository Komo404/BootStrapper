namespace BootStrapper.Core.Models
{
    public class UserConfig
    {
        public required string UnityPath { get; set; } = "C:/UnityProjects";
        public required string ProjectsFolder { get; set; }
        public required string TemplatesFolder { get; set; }
        public required string Theme { get; set; }
        public required string AutoUpdateEnabled { get; set; }
    }
}