namespace BootStrapper.Core.Models
{
    public class TemplateManifest
    {
        // Manifest
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string Version { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public required string minUnityVersion { get; set; }
        public required string maxUnityVersion { get; set; }
        public List<string> Tags { get; set; } = new(); // Ex: "Audio, Input System, etc"
        public string TemplatePath { get; set; } 

    }
}