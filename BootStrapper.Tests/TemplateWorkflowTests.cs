using BootStrapper.Core.Models;
using BootStrapper.Core.Services;

namespace BootStrapper.Tests;

public sealed class TemplateWorkflowTests : IDisposable
{
    private readonly string _root = Path.Combine(Path.GetTempPath(), "BootStrapperTests", Guid.NewGuid().ToString());

    public TemplateWorkflowTests() => Directory.CreateDirectory(_root);

    [Fact]
    public void SelectedFilesAreCopiedForEachVersionAndIntoExistingProject()
    {
        string source = Path.Combine(_root, "Source");
        Directory.CreateDirectory(Path.Combine(source, "Nested"));
        File.WriteAllText(Path.Combine(source, "Root.cs"), "root script");
        File.WriteAllText(Path.Combine(source, "Nested", "Keep.cs"), "keep script");
        File.WriteAllText(Path.Combine(source, "Nested", "Discard.cs"), "discard script");
        var tree = ScriptTreeService.BuildScriptTree(source, source);
        var nested = tree.Single(node => node.Name == "Nested");
        Assert.True(ScriptTreeService.RemoveTreeNode(tree, nested.Children.Single(node => node.Name == "Discard.cs")));

        var config = Config();
        var template = TemplateService.CreateTemplate(config, new TemplateManifest
        {
            Name = "Example", Description = "", Category = default,
            UnityVersions = ["version-a", "version-b"], TemplatePath = "", ManifestPath = ""
        }, tree);

        foreach (string version in template.UnityVersions)
        {
            string path = Path.Combine(template.TemplatePath, version);
            Assert.Equal("root script", File.ReadAllText(Path.Combine(path, "Root.cs")));
            Assert.True(File.Exists(Path.Combine(path, "Nested", "Keep.cs")));
            Assert.False(File.Exists(Path.Combine(path, "Nested", "Discard.cs")));
        }
        Assert.True(File.Exists(Path.Combine(source, "Nested", "Discard.cs")));

        var project = Project(template.Id, "version-a");
        Directory.CreateDirectory(project.UnityProjectPath);
        ProjectService.CreateProjectManifestAndCopyTemplates(config, project);
        Assert.True(File.Exists(Path.Combine(project.UnityProjectPath, "Assets", "Scripts", "Root.cs")));
        Assert.Single(ProjectService.ListProjects(config));
        ProjectService.UnlistProject(project);
        Assert.Empty(ProjectService.ListProjects(config));
        Assert.True(Directory.Exists(project.UnityProjectPath));
    }

    [Fact]
    public void MissingTemplateDoesNotRegisterProject()
    {
        var project = Project(Guid.NewGuid(), "missing");
        Directory.CreateDirectory(project.UnityProjectPath);
        Assert.ThrowsAny<Exception>(() => ProjectService.CreateProjectManifestAndCopyTemplates(Config(), project));
        Assert.False(File.Exists(Path.Combine(project.UnityProjectPath, "manifest.json")));
    }

    private UserConfig Config()
    {
        string templates = Path.Combine(_root, "Templates");
        string projects = Path.Combine(_root, "Projects");
        Directory.CreateDirectory(templates);
        Directory.CreateDirectory(projects);
        return new UserConfig
        {
            BootStrapperFolderPath = _root, TemplatesFolderPath = templates, UnityProjectsFolderPath = projects,
            AutoLaunchProject = false, AutoUpdateEnabled = false
        };
    }

    private ProjectManifest Project(Guid templateId, string version) => new()
    {
        Name = "Project", Description = "", UnityVersion = version,
        UnityProjectPath = Path.Combine(_root, "Projects", "Project"), TemplateIds = [templateId]
    };

    public void Dispose() => Directory.Delete(_root, true);
}
