using BootStrapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Core.Service;

public static class ConfigService
{
    public static UserConfig LoadConfig(string configFilePath) // é chamado pelo App.axaml.cs* para carregar a configuração do usuário
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
        if (config == null) {
            CreateDefaultConfig(configFilePath);
        }

        if (string.IsNullOrEmpty(configFilePath)) {
            throw new ArgumentNullException(nameof(configFilePath), "Config file path cannot be null or empty.");
        }

        string? directory = Path.GetDirectoryName(configFilePath);

        if (!Directory.Exists(directory))
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

        List<string> unityPaths = UnityService.GetUnityPaths();

        UserConfig defaultConfig = new UserConfig
        {
            UnitySelectedPath = unityPaths[0],
            UnityPaths = unityPaths,
            Theme = "Dark",
            ProjectsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BootStrapperProjects"),
            TemplatesFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "BootStrapperTemplates"),
            AutoUpdateEnabled = true
        };

        if (!Directory.Exists(defaultConfig.ProjectsFolder))
            Directory.CreateDirectory(defaultConfig.ProjectsFolder);
        if (!Directory.Exists(defaultConfig.TemplatesFolder))
            Directory.CreateDirectory(defaultConfig.TemplatesFolder);

        SaveConfig(configFilePath, defaultConfig);

    }
}
