using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.Core.Service;

public class UnityService
{
    public void CreateUnityProject(string unityPath, string projectPath)
    {
        if (string.IsNullOrEmpty(unityPath))
            throw new ArgumentNullException(nameof(unityPath), "Unity path cannot be null or empty.");
        if (string.IsNullOrEmpty(projectPath))
            throw new ArgumentNullException(nameof(projectPath), "Project path cannot be null or empty.");
        
        string command = $"\"{unityPath}\" -createProject \"{projectPath}\""; // Cria um projeto Unity

        try
        {
            System.Diagnostics.Process.Start("cmd.exe", $"/C {command}");
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Unity project.", ex);
        }
    }

    public void OpenUnityProject(string unityPath, string projectPath)
    {
        if (string.IsNullOrEmpty(unityPath))
            throw new ArgumentNullException(nameof(unityPath), "Unity path cannot be null or empty.");
        if (string.IsNullOrEmpty(projectPath))
            throw new ArgumentNullException(nameof(projectPath), "Project path cannot be null or empty.");
       
        string command = $"\"{unityPath}\" -projectPath \"{projectPath}\""; // Abrir um projeto Unity

        try {
            System.Diagnostics.Process.Start("cmd.exe", $"/C {command}");
        }
        catch (Exception ex) {
            throw new Exception("Failed to open Unity project.", ex);
        }
    }

    public void GetUnityVersion(string unityPath)
    {
        if (string.IsNullOrEmpty(unityPath))
            throw new ArgumentNullException(nameof(unityPath), "Unity path cannot be null or empty.");

        string command = $"\"{unityPath}\" -version"; // Versão unity
        try {
            System.Diagnostics.Process.Start("cmd.exe", $"/C {command}");
        }
        catch (Exception ex) {
            throw new Exception("Failed to get Unity version.", ex);
        }
    }

    public string GetUnityPath()
    {
        string unityPath = @"C:\Program Files\Unity\Hub\Editor\2021.3.0f1\Editor\Unity.exe"; // Eu não sei o que estou fazendo
        if (!File.Exists(unityPath))
            throw new FileNotFoundException("Unity executable not found at the specified path.", unityPath);

        return unityPath;
    }
}