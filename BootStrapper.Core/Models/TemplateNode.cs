namespace BootStrapper.Core.Models;

public class TemplateNode
{
    public required string Name { get; set; } = string.Empty;
    public required bool IsFolder { get; set; } = false;
    public required string UserScriptFolderPath { get; set; } = string.Empty;
    public required string? RelativePath { get; set; } = string.Empty;
    public required List<TemplateNode> Children { get; set; } = [];
}