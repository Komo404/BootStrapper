using BootStrapper.Core.Models;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace BootStrapper.Core.Service;

public class ProjectService
{
    public static void CreateProject(UserConfig config, Project projectInfo)
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
    }

    public static void DeleteProject(Project project)
    {
        if (!Directory.Exists(project.Path))
            throw new ArgumentNullException("Project is not existent");

        Directory.Delete(project.Path, true);
    }

    public static void UpdateProject(UserConfig config, string projectPath, Project projectInfo)
    {
        if (!Directory.Exists(projectPath))
            throw new ArgumentNullException("Project is not existent");

        string projectManifestPath = Path.Combine(projectPath, "manifest.json");

        ManifestService.WriteManifest(projectManifestPath, projectInfo);

        string scriptsFolderPath = Path.Combine(projectPath, "Scripts");
        // TODO: Atualiza os arquivos do projeto, caso haja templates novos
    }

    public static Project GetProject(UserConfig config, string projectPath)
    {
        if (!Directory.Exists(projectPath))
            throw new ArgumentNullException("Project is not existent");

        List<Project> projects = ListProjects(config);

        foreach (Project project in projects)
        {
            if (project.Path == projectPath)
                return project;
        }

        throw new Exception("Não achou o projeto");
    }

    public static List<Project> ListProjects(UserConfig config)
    {

        string[] folders = Directory.GetDirectories(config.ProjectsFolder);
        List<Project> projects = new List<Project>();

        foreach (string folder in folders)
        {
            if (!Directory.Exists(folder))
                continue;
            else if (!File.Exists(Path.Combine(folder, "manifest.json")))
                continue;
            string manifestPath = Path.Combine(folder, "manifest.json");
            Project project = ManifestService.ReadManifest<Project>(manifestPath);
            projects.Add(project);
        }

        return projects;
    }

    public static string[] GetProjectHistory(UserConfig config, string projectPath)
    {
        Project project = GetProject(config, projectPath);
        String[] changeHistory;
        if (project.ChangeHistory != null)
        {
            changeHistory = project.ChangeHistory;
            return changeHistory;
        }

        throw new Exception("Project doesn't have change history");
    }

    public static void OpenProjectFolder()
    {

    }
}
