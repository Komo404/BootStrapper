using System.Collections.ObjectModel;

namespace BootStrapper.Core.Models;
public class ProjectManifest
{
    public Guid Id { get; set; } = Guid.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public required string UnityVersion { get; set; } = string.Empty;
    public required string MetadataPath { get; set; } = string.Empty;
    public required string UnityProjectPath {  get; set; } = string.Empty;
    public required List<Guid> TemplateIds { get; set; } = [];
}