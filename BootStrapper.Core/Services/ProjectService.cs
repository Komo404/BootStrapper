using BootStrapper.Core.Models;
using BootStrapper.Core.Helpers;

namespace BootStrapper.Core.Services;

public static class ProjectService
{
    public static void CreateProjectManifestAndCopyTemplates(UserConfig config, ProjectManifest project)
    {
        string manifestPath = Path.Combine(project.UnityProjectPath, "manifest.json");

        string unityPath = Path.Combine(project.UnityProjectPath, "Assets", "Scripts");
        Directory.CreateDirectory(unityPath);

        List<string> TemplatePaths = [];
        foreach (var id in project.TemplateIds)
            TemplatePaths.Add(TemplateService.GetTemplateById(config, id).TemplatePath);

        var sources = TemplatePaths.Select(path => Path.Combine(path, project.UnityVersion)).ToList();
        foreach (string source in sources)
            if (!Directory.Exists(source))
                throw new DirectoryNotFoundException($"Template version directory not found: {source}");

        foreach (string source in sources)
            FileSystemHelper.CopyDirectoryRecursively(source, unityPath);

        ManifestHelper.WriteManifest(manifestPath, project);
    }

    public static void UnlistProject(ProjectManifest project)
    {
        if (project == null) throw new ArgumentNullException("Project is not existent");
        string manifestPath = Path.Combine(project.UnityProjectPath, "manifest.json");

        if (File.Exists(manifestPath))
            File.Delete(manifestPath);
    }

    public static void UpdateProject(ProjectManifest projectInfo)
    {
        if (projectInfo == null) throw new ArgumentNullException("Project is not existent");
        ManifestHelper.WriteManifest(Path.Combine(projectInfo.UnityProjectPath, "manifest.json"), projectInfo);
    }

    public static List<ProjectManifest> ListProjects(UserConfig config)
    {
        string[] folders = Directory.GetDirectories(config.UnityProjectsFolderPath, "*", SearchOption.TopDirectoryOnly);
        List<ProjectManifest> projects = [];

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder)) continue; // just in case the folder was deleted between the time we got the list and now
            else if (!File.Exists(Path.Combine(folder, "manifest.json"))) continue;

            string manifestPath = Path.Combine(folder, "manifest.json");
            ProjectManifest project = ManifestHelper.ReadManifest<ProjectManifest>(manifestPath);
            projects.Add(project);
        }

        return projects;
    }
}
