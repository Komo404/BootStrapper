using BootStrapper.Core.Models;
using BootStrapper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text;

namespace BootStrapper.Core.Services;

public class ProjectService
{
    public static void CreateProject(UserConfig config, ProjectManifest projectInfo)
    {
        if (Directory.Exists(projectInfo.MetadataPath))
            throw new Exception("A project with --Wha, What? THIS NAME? Exists.");

        Directory.CreateDirectory(projectInfo.MetadataPath);

        string manifestPath = Path.Combine(projectInfo.MetadataPath, "manifest.json");
        ManifestService.WriteManifest(manifestPath, projectInfo);

        string unityPath = Path.Combine(projectInfo.UnityProjectPath, "Assets", "Scripts");
        Directory.CreateDirectory(unityPath);

        List<string> TemplatePaths = [];
        foreach (var id in projectInfo.TemplateIds)
            TemplatePaths.Add(TemplateService.GetTemplateById(config, id).TemplatePath);

        foreach (var templatePath in TemplatePaths)
            FileSystemHelper.CopyDirectoryRecursively(Path.Combine(templatePath, projectInfo.UnityVersion), unityPath);
    }

    public static void DeleteProject(ProjectManifest project)
    {
        if (!Directory.Exists(project.UnityProjectPath) || !Directory.Exists(project.MetadataPath))
            throw new ArgumentNullException("Project is not existent");

        Directory.Delete(project.MetadataPath, true);
        Directory.Delete(project.UnityProjectPath, true);
    }

    public static void UpdateProject(ProjectManifest projectInfo)
    {
        if (projectInfo == null) throw new ArgumentNullException("Project is not existent");
        ManifestService.WriteManifest(Path.Combine(projectInfo.MetadataPath, "manifest.json"), projectInfo);
    }

    public static List<ProjectManifest> ListProjects(UserConfig config)
    {
        string[] folders = Directory.GetDirectories(config.ProjectsFolder);
        List<ProjectManifest> projects = [];

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
                continue;
            else if (!File.Exists(Path.Combine(folder, "manifest.json")))
                continue;
            string manifestPath = Path.Combine(folder, "manifest.json");
            ProjectManifest project = ManifestService.ReadManifest<ProjectManifest>(manifestPath);
            projects.Add(project);
        }

        return projects;
    }
}
