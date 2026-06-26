namespace BootStrapper.Core.Models
{
    public class TemplateManifest
    {
        public Guid Id { get; set; } = Guid.Empty;
        public required string Name { get; set; } = string.Empty;
        public required TemplateCategory Category { get; set; }
        public required string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public required List<string> UnityVersions { get; set; } = [];
        public List<string> Tags { get; set; } = [];
        public required string TemplatePath { get; set; } = string.Empty;
        public required string ManifestPath { get; set; } = string.Empty;
    }
}