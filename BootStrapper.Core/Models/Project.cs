namespace BootStrapper.Core.Models;
public class Project
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime CreationDate { get; set; } = DateTime.Now;
    public string Author { get; set; }
    public required string Path { get; set; }
    public required string UnityVersion { get; set; }
    public required string[] Templates { get; set; } // Apenas nomes?
    public string[]? ChangeHistory { get; set; }
}