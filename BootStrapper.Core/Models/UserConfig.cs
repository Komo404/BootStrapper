namespace BootStrapper.Core.Models
{
    public class UserConfig
    {
        public Guid Id { get; set; }
        public required string unityPath { get; set; }
        public required string projectsPath { get; set; }
        public required string updPreference { get; set; }
    }
}