using BootStrapper.Core.Models;
using BootStrapper.Core.Helpers;

namespace BootStrapper.Core.Services;
public static class ConfigService
{
    static string bootStrapperFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BootStrapper");
    public static UserConfig LoadConfig(string configFilePath) // Called by App.axaml.cs
    {
        if (!File.Exists(configFilePath))
            CreateDefaultConfig(configFilePath);

        if (string.IsNullOrEmpty(configFilePath))
            throw new ArgumentNullException(nameof(configFilePath), "Config file path null or empty");

        string json = File.ReadAllText(configFilePath);
        UserConfig config = System.Text.Json.JsonSerializer.Deserialize<UserConfig>(json) ?? throw new Exception("Failed to deserialize config.");
        return config;
    }
    public static void SaveConfig(string configFilePath, UserConfig config)
    {
        if (string.IsNullOrEmpty(configFilePath))
        {
            throw new ArgumentNullException(nameof(configFilePath), "Config file path cannot be null or empty.");
        }

        ArgumentNullException.ThrowIfNull(config);

        string? directory = Path.GetDirectoryName(configFilePath);

        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        string json = System.Text.Json.JsonSerializer.Serialize(config);
        File.WriteAllText(configFilePath, json);
    }
    public static void CreateDefaultConfig(string configFilePath)
    {
        if (string.IsNullOrEmpty(configFilePath))
            throw new ArgumentNullException(nameof(configFilePath), "Config file path cannot be null or empty.");

        UserConfig defaultConfig = new UserConfig
        {
            UnityProjectsFolderPath = GetPathsHelper.GetUnityProjectsFolderPath(),
            BootStrapperFolderPath = bootStrapperFolderPath,
            TemplatesFolderPath = Path.Combine(bootStrapperFolderPath, "Templates"),
            AutoLaunchProject = true,
            AutoUpdateEnabled = true
        };

        if (!Directory.Exists(defaultConfig.TemplatesFolderPath))
            Directory.CreateDirectory(defaultConfig.TemplatesFolderPath);

        SaveConfig(configFilePath, defaultConfig);
    }
}