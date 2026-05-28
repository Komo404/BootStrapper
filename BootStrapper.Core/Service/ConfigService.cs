using BootStrapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Core.Service
{
    public static class ConfigService
    {

        public static UserConfig LoadConfig(string configFilePath)
        {
            if (string.IsNullOrEmpty(configFilePath))
                throw new ArgumentNullException(nameof(configFilePath), "Config file path cannot be null or empty.");
            if (!File.Exists(configFilePath))
                throw new FileNotFoundException("Config file not found.", configFilePath);

            string json = File.ReadAllText(configFilePath);
            UserConfig config = System.Text.Json.JsonSerializer.Deserialize<UserConfig>(json) ?? throw new Exception("Failed to deserialize config.");
            return config;
        }

        public static void SaveConfig(UserConfig config, string configFilePath)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config), "Config cannot be null.");
            if (string.IsNullOrEmpty(configFilePath))
                throw new ArgumentNullException(nameof(configFilePath), "Config file path cannot be null or empty.");

            string json = System.Text.Json.JsonSerializer.Serialize(config);
            File.WriteAllText(configFilePath, json);
        }

    }
}
