namespace BootStrapper.Core.Models;
public class Project
{
    public Guid Id { get; set; } = Guid.Empty;
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string Author { get; set; } = string.Empty;
    public required string Path { get; set; } = string.Empty;
    public required string UnityVersion { get; set; } = string.Empty;
    public required List<TemplateNode> Templates { get; set; } = [];
    public string[]? ChangeHistory { get; set; } = [];
}