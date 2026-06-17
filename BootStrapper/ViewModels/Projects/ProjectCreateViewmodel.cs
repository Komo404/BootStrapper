using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.ViewModels.Templates;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string UnityVersion { get; set; } = string.Empty;
    public string[] Tags { get; set; } = [];
    public List<TemplateManifest> Templates { get; } = [];
    public ObservableCollection<Guid> TemplatesAdded { get; set; } = [];
    public string Author { get; set; } = string.Empty;

    public ProjectCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        Templates = TemplateService.GetAllTemplates(_config);
    }

    public ProjectCreateViewModel() {
        Templates = new List<TemplateManifest>()
        {
            new TemplateManifest
            {
                Name = "Mock1",
                Description = "",
                Version = "1.0",
                UnityVersion = "2022.3",
                MaxUnityVersion = "2023.0",
                Author = "",
                Tags = new List<string>(),
                TemplatePath = "",
                ManifestPath = "",
            },
            new TemplateManifest
            {
                Name = "Mock2",
                Description = "",
                Version = "1.0",
                UnityVersion = "2022.3",
                MaxUnityVersion = "2023.0",
                Author = "",
                Tags = new List<string>(),
                TemplatePath = "",
                ManifestPath = "",
            },
            new TemplateManifest
            {
                Name = "Mock3",
                Description = "",
                Version = "1.0",
                UnityVersion = "2022.3",
                MaxUnityVersion = "2023.0",
                Author = "",
                Tags = new List<string>(),
                TemplatePath = "",
                ManifestPath = "",
            }
        };
    }

    [RelayCommand]
    private void GoToProjectCreate()
    {
        _navigation.CurrentView = new ProjectCreateViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void GoToTemplateCreate()
    {
        _navigation.CurrentView = new TemplateCreateViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void AddTemplate(Guid template)
    {
        TemplatesAdded.Add(template);
    }

    [RelayCommand]
    private void CreateProject(ProjectManifest projectInfo)
    {
        ProjectManifest newProject = new ProjectManifest
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Description  = Description,
            CreationDate = DateTime.Now,
            Path = Path,
            UnityVersion = UnityVersion,
            Author = Author,
            TemplateIds = TemplatesAdded.ToList(),
            ChangeHistory = Array.Empty<string>()
        };

        ProjectService.CreateProject(_config, newProject);
        _navigation.CurrentView = new ProjectInfoViewModel(_navigation, newProject, _config);
    }

}
