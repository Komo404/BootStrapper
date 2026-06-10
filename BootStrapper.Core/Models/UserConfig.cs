namespace BootStrapper.Core.Models
{
    public class UserConfig
    {
        public List<string> UnityPaths { get; set; } = [];
        public string UnitySelectedPath { get; set; } = string.Empty;
        public required string ProjectsFolder { get; set; } = string.Empty;
        public required string TemplatesFolder { get; set; } = string.Empty;
        public required string Theme { get; set; } = string.Empty;
        public required bool AutoUpdateEnabled { get; set; } = true;
    }
}