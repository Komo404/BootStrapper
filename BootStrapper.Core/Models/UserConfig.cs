namespace BootStrapper.Core.Models
{
    public class UserConfig
    {
        public List<string> UnityPaths { get; set; }
        public string UnitySelectedPath { get; set; }
        public required string ProjectsFolder { get; set; }
        public required string TemplatesFolder { get; set; }
        public required string Theme { get; set; }
        public required bool AutoUpdateEnabled { get; set; }
    }
}