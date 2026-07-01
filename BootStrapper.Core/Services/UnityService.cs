using BootStrapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace BootStrapper.Core.Services;

public static class UnityService
{
    public static async Task CreateUnityProjectAsync(UserConfig config, ProjectManifest project)
    {
        if (Directory.Exists(project.UnityProjectPath))
            throw new Exception("Projeto unity encontrado com ESSE NOME????");

        Directory.CreateDirectory(project.UnityProjectPath);

        string unityExePath = GetUnityVersionPath(config, project.UnityVersion);

        var processStartInfo = new ProcessStartInfo
        {
            FileName = unityExePath,
            ArgumentList = { "-createProject", project.UnityProjectPath },
            UseShellExecute = false
        };

        try
        {
            var process = Process.Start(processStartInfo);

            string assetsPath = Path.Combine(project.UnityProjectPath, "Assets");
            string projectSettingsPath = Path.Combine(project.UnityProjectPath, "ProjectSettings");

            var timeout = TimeSpan.FromSeconds(60);
            var stopwatch = Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (Directory.Exists(assetsPath) && Directory.Exists(projectSettingsPath)) return;
                await Task.Delay(500);
            }

            throw new TimeoutException("Unity project creation timed out.");
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Unity project.", ex);
        }
    }

    public static void OpenUnityProject(UserConfig config, ProjectManifest project)
    {
        if (string.IsNullOrEmpty(project.UnityProjectPath))
            throw new ArgumentNullException(nameof(project.UnityProjectPath), "Project path cannot be null or empty.");

        string unityExePath = GetUnityVersionPath(config, project.UnityVersion);

        var processStartInfo = new ProcessStartInfo
        {
            FileName = unityExePath,
            ArgumentList = { "-projectPath", project.UnityProjectPath },
            UseShellExecute = false
        };

        try {
            Process.Start(processStartInfo);
        }
        catch (Exception ex) {
            throw new Exception("Failed to open Unity project.", ex);
        }
    }
    public static List<string> GetUnityVersions(UserConfig config)
    {
        string unityEditorsPath = GetUnityEditorsPath(config);

        if (!Directory.Exists(unityEditorsPath))
            throw new DirectoryNotFoundException($"Unity Editors directory not found at the specified path: {unityEditorsPath}");

        List<string> unityVersions = [];

        foreach (string directory in Directory.GetDirectories(unityEditorsPath))
        {
            string exePath = Path.Combine(directory, "Editor", "Unity.exe");

            if (File.Exists(exePath))
                unityVersions.Add(Path.GetFileName(directory));
        }

        return unityVersions;
    }

    private static string GetUnityEditorsPath(UserConfig config)
    {
        if (!string.IsNullOrEmpty(config.CustomUnityEditorsPath))
            return config.CustomUnityEditorsPath;

        return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Unity", "Hub", "Editor");
    }

    private static string GetUnityVersionPath(UserConfig config, string unityVersion)
    {
        string editorsRoot = GetUnityEditorsPath(config);
        string exePath = Path.Combine(editorsRoot, unityVersion, "Editor", "Unity.exe");

        if (!File.Exists(exePath))
            throw new FileNotFoundException($"Unity {unityVersion} executable not found at {exePath}");

        return exePath;
    }
}