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
        public required string UnityVersion { get; set; }
        public string MaxUnityVersion { get; set; }
        public string Author { get; set; }
        public List<string> Tags { get; set; } = new(); // Ex: "Audio, Input System, etc"
        public required string TemplatePath { get; set; } 
        public required string ManifestPath { get; set; }
    }
}