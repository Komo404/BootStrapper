namespace BootStrapper.Core.Models;


public class UserConfig
{
    public string UnityProjectsFolderPath { get; set; } = string.Empty;
    public required string BootStrapperFolderPath { get; set; } = string.Empty;
    public required string TemplatesFolderPath { get; set; } = string.Empty;
    public required bool AutoLaunchProject { get; set; } = true;
    public required bool AutoUpdateEnabled { get; set; } = true;
}