using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly ProjectManifest _project;
    private readonly UserConfig _config;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> UnityVersions { get; set; } = [];
    public string UnityVersion { get; set; } = string.Empty;
    public ObservableCollection<TemplateManifest> TemplatePreviewInfo { get; set; } = [];
    public string[] ProjectFiles { get; set; } = { };

    public ProjectInfoViewModel(NavigationService navigation, ProjectManifest project, UserConfig config)
    {
        _navigation = navigation;
        _project = project; // Supondo que o project já foi carregado na HomeViewModel, então não precisa carregar de novo
        _config = config;

        Name = _project.Name;
        Description = _project.Description;
        UnityVersion = _project.UnityVersion;

    }

    public ProjectInfoViewModel()
    {
        Name = "Mock Project";
        Description = "Description";
        UnityVersion = "6000.0";
        TemplatePreviewInfo = new ObservableCollection<TemplateManifest>
        {
            new TemplateManifest {
                Name = "TemplateTeste",
                Description = "Teste",
                Category = new TemplateCategory(),
                TemplatePath = "",
                ManifestPath = "",
                UnityVersions = ["1.11.f1"],
            }
        };
    }

    private void GetProjectFiles()
    {
        var projectFiles = new List<string>();
    }

    [RelayCommand]
    private void OpenProject() => UnityService.OpenUnityProject(_config, _project);

    [RelayCommand]
    private void DeleteProject()
    {
        ProjectService.DeleteProject(_project);
        _navigation.CurrentView = new ProjectListViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void AddTemplate(TemplateNode template)
    {

    }

    [RelayCommand]
    private void OpenProjectFolder() => _navigation.Explorer.OpenBootStrapperFolder(_project.Path);

    [RelayCommand]
    private void UpdateProject()
    {

    }
}
