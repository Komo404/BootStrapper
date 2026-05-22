namespace BootStrapper.Core.Models
{
    public class Template
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

        // FileSystem
        public List<Script> ScriptStructure { get; set; }
        public string TemplatePath { get; set; }
        public string ManifestPath { get; set; }
    }
}