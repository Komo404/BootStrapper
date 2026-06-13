using BootStrapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace BootStrapper.Core.Service;

public static class UnityService
{
    public static void CreateUnityProject(UserConfig config, Project project)
    {
        if (string.IsNullOrEmpty(config.UnitySelectedPath))
            throw new ArgumentNullException(nameof(config.UnitySelectedPath), "Unity path cannot be null or empty.");
        if (string.IsNullOrEmpty(project.Path))
            throw new ArgumentNullException(nameof(project.Path), "Project path cannot be null or empty.");
        
        string command = $"\"{config.UnitySelectedPath}\" -createProject \"{project.Path}\""; // Cria um projeto Unity

        try
        {
            System.Diagnostics.Process.Start("cmd.exe", $"/C {command}");
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Unity project.", ex);
        }
    }

    public static void OpenUnityProject(UserConfig config, Project project)
    {
        if (string.IsNullOrEmpty(config.UnitySelectedPath))
            throw new ArgumentNullException(nameof(config.UnitySelectedPath), "Unity path cannot be null or empty.");
        if (string.IsNullOrEmpty(project.Path))
            throw new ArgumentNullException(nameof(project.Path), "Project path cannot be null or empty.");
       
        string command = $"\"{config.UnitySelectedPath}\" -projectPath \"{project.Path}\""; // Abrir um projeto Unity

        try {
            System.Diagnostics.Process.Start("cmd.exe", $"/C {command}");
        }
        catch (Exception ex) {
            throw new Exception("Failed to open Unity project.", ex);
        }
    }
    public static List<string> GetUnityVersions()
    {
        string unityHubPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Unity", "Hub", "Editor");

        if (!Directory.Exists(unityHubPath))
            throw new DirectoryNotFoundException($"Unity Hub directory not found at the specified path: {unityHubPath}");

        List<string> unityVersions = new List<string>();
        string[] versionDirectories = Directory.GetDirectories(unityHubPath);
        foreach (string versionDir in versionDirectories)
        {
            string versionName = Path.GetFileName(versionDir);
            unityVersions.Add(versionName);
        }
        return unityVersions;
    }

    public static List<string> GetUnityPaths()
    {
        string unityHubPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Unity", "Hub", "Editor");

        if (!Directory.Exists(unityHubPath))
            throw new DirectoryNotFoundException($"Unity Hub directory not found at the specified path: {unityHubPath}");

        List<string> unityPaths = new List<string>();
        string[] versionDirectories = Directory.GetDirectories(unityHubPath);
        foreach (string versionDir in versionDirectories)
        {
            string unityExePath = Path.Combine(versionDir, "Editor", "Unity.exe");
            if (File.Exists(unityExePath))
            {
                unityPaths.Add(unityExePath);
            }
        }
        return unityPaths;
    }
}