namespace BootStrapper.Core.Models
{
    public class TemplateManifest
    {
        // Manifest
        public Guid Id { get; set; } = Guid.Empty;
        public required string Name { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
        public required string Version { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public required string UnityVersion { get; set; } = string.Empty;
        public string MaxUnityVersion { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
        public required string TemplatePath { get; set; } = string.Empty;
        public required string ManifestPath { get; set; } = string.Empty;
    }
}