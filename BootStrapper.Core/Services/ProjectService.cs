using BootStrapper.Core.Models;
using BootStrapper.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;
using System.Text;

namespace BootStrapper.Core.Services;

public class ProjectService
{
    public static void CreateProject(UserConfig config, ProjectManifest projectInfo)
    {
        if (config.ProjectsFolder == null)
            throw new DirectoryNotFoundException("Projects Folder is null");

        projectInfo.Path = Path.Combine(config.ProjectsFolder, projectInfo.Id.ToString());

        Directory.CreateDirectory(projectInfo.Path); // Nome do projeto deve estar no path

        string manifestPath = Path.Combine(projectInfo.Path, "manifest.json");
        File.Create(manifestPath).Close();
        ManifestService.WriteManifest(manifestPath, projectInfo);

        string scriptsFolderPath = Path.Combine(projectInfo.Path, "Scripts");
        Directory.CreateDirectory(scriptsFolderPath);

        List<string> TemplatePaths = [];
        foreach (var id in projectInfo.TemplateIds)
        {
            TemplatePaths.Add(TemplateService.GetTemplateById(config, id).TemplatePath);
            foreach (var templatePath in TemplatePaths)
            {
                FileSystemHelper.CopyDirectoryRecursively(Path.Combine(templatePath, projectInfo.UnityVersion), scriptsFolderPath);
            }
        }
    }

    public static void DeleteProject(ProjectManifest project)
    {
        if (!Directory.Exists(project.Path))
            throw new ArgumentNullException("Project is not existent");

        Directory.Delete(project.Path, true);
    }

    public static void UpdateProject(UserConfig config, string projectPath, ProjectManifest projectInfo)
    {
        if (!Directory.Exists(projectPath))
            throw new ArgumentNullException("Project is not existent");

        string projectManifestPath = Path.Combine(projectPath, "manifest.json");

        ManifestService.WriteManifest(projectManifestPath, projectInfo);

        string scriptsFolderPath = Path.Combine(projectPath, "Scripts");
        // TODO: Atualiza os arquivos do projeto, caso haja templates novos
    }

    public static List<ProjectManifest> ListProjects(UserConfig config)
    {

        string[] folders = Directory.GetDirectories(config.ProjectsFolder);
        List<ProjectManifest> projects = new List<ProjectManifest>();

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

    public static void OpenUnityProjectFolder()
    {
        
    }
}
