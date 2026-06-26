namespace BootStrapper.Core.Models
{
    public class UserConfig // Configs do usuário ficam
    {
        public string CustomUnityEditorsPath { get; set; } = string.Empty;
        public required string ProjectsFolder { get; set; } = string.Empty;
        public required string TemplatesFolder { get; set; } = string.Empty;
        public required bool AutoLaunchProject { get; set; } = true;
        public required bool AutoUpdateEnabled { get; set; } = true;
    }
}