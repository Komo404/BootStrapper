using System.Collections.ObjectModel;

namespace BootStrapper.Core.Models;
public class Project
{
    public Guid Id { get; set; } = Guid.Empty;
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public required string Name { get; set; } = string.Empty;
    public required string Description { get; set; } = string.Empty;
    public required string UnityVersion { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public required string Path { get; set; } = string.Empty;
    public required ObservableCollection<TemplateNode> Templates { get; set; } = [];
    public string[]? ChangeHistory { get; set; } = [];
}