using BootStrapper.Core.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.Json;

namespace BootStrapper.Core.Services;

public static class UnityService
{
    private const string UnityCliExecutable = "unity";

    public static async Task CreateUnityProjectAsync(ProjectManifest project)
    {
        ArgumentNullException.ThrowIfNull(project);

        if (string.IsNullOrWhiteSpace(project.UnityProjectPath))
            throw new ArgumentException("Project path cannot be null or empty.", nameof(project));

        if (string.IsNullOrWhiteSpace(project.UnityVersion))
            throw new ArgumentException("Unity version cannot be null or empty.", nameof(project));

        string projectPath = Path.GetFullPath(project.UnityProjectPath);
        if (Directory.Exists(projectPath) || File.Exists(projectPath))
            throw new IOException($"A file or directory already exists at the project path: {projectPath}");

        string projectName = Path.GetFileName(projectPath);
        string parentDirectory = Path.GetDirectoryName(projectPath) ?? throw new ArgumentException("Project path must include a parent directory.", nameof(project));

        await RunUnityCliAsync(
            "projects", "create", projectName,
            "--path", parentDirectory,
            "--editor-version", project.UnityVersion,
            "--no-cloud",
            "--non-interactive",
            "--no-banner");

        if (!Directory.Exists(projectPath))
            throw new IOException($"Unity CLI reported success but did not create the project directory: {projectPath}");
    }

    public static async Task OpenUnityProjectAsync(ProjectManifest project)
    {
        ArgumentNullException.ThrowIfNull(project);
        if (string.IsNullOrWhiteSpace(project.UnityProjectPath))
            throw new ArgumentException("Project path cannot be null or empty.", nameof(project));

        await RunUnityCliAsync(
            "open", Path.GetFullPath(project.UnityProjectPath),
            "--non-interactive",
            "--no-banner");
    }

    public static void DeleteUnityProject(ProjectManifest project)
    {
        ArgumentNullException.ThrowIfNull(project);

        if (!Directory.Exists(project.UnityProjectPath))
            throw new DirectoryNotFoundException($"Project directory not found: {project.UnityProjectPath}");

        Directory.Delete(project.UnityProjectPath, true);
    }

    public static async Task<List<string>> GetUnityVersionsAsync()
    {
        string output = await RunUnityCliAsync(
            "editors",
            "--installed",
            "--json",
            "--no-pager",
            "--no-banner");

        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement root = document.RootElement;
        if (!root.TryGetProperty("success", out JsonElement success) || success.ValueKind != JsonValueKind.True || !root.TryGetProperty("data", out JsonElement data) || !data.TryGetProperty("items", out JsonElement items))
        {
            throw new InvalidDataException("Unity CLI returned an invalid installed Editors response.");
        }

        if (items.ValueKind != JsonValueKind.Array)
            throw new InvalidDataException("Unity CLI returned an invalid installed Editors list.");

        List<string> versions = [];
        foreach (JsonElement item in items.EnumerateArray()) {
            if (!item.TryGetProperty("version", out JsonElement version) || version.ValueKind != JsonValueKind.String)
            {
                throw new InvalidDataException("Unity CLI returned an Editor without a valid version.");
            }

            string installedVersion = version.GetString()
                ?? throw new InvalidDataException("Unity CLI returned an Editor without a valid version.");
            if (string.IsNullOrWhiteSpace(installedVersion))
                throw new InvalidDataException("Unity CLI returned an Editor without a valid version.");

            versions.Add(installedVersion);
        }

        return versions;
    }

    public static List<string> GetUnityVersions() => GetUnityVersionsAsync().GetAwaiter().GetResult();

    private static async Task<string> RunUnityCliAsync(params string[] arguments)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = UnityCliExecutable,
            UseShellExecute = false,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            CreateNoWindow = true
        };

        foreach (string argument in arguments)
            startInfo.ArgumentList.Add(argument);

        using var process = new Process { StartInfo = startInfo };

        try
        {
            if (!process.Start())
                throw new InvalidOperationException("Unity CLI could not be started.");
        }
        catch (Win32Exception ex)
        {
            throw new InvalidOperationException("Unity CLI executable 'unity' was not found. Install Unity CLI and make sure it is available on PATH.", ex);
        }

        Task<string> standardOutputTask = process.StandardOutput.ReadToEndAsync();
        Task<string> standardErrorTask = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync().ConfigureAwait(false);

        string standardOutput = await standardOutputTask.ConfigureAwait(false);
        string standardError = await standardErrorTask.ConfigureAwait(false);

        if (process.ExitCode != 0)
        {
            string details = string.IsNullOrWhiteSpace(standardError) ? standardOutput : standardError;
            throw new InvalidOperationException($"Unity CLI command failed with exit code {process.ExitCode}: {details.Trim()}");
        }

        return standardOutput;
    }
}
