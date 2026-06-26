using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.ComponentModel;
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

    [ObservableProperty] public string name = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> UnityVersions { get; set; } = [];
    public string UnityVersion { get; set; } = string.Empty;
    [ObservableProperty] public ObservableCollection<string> projectTemplates = [];
    public ObservableCollection<TemplateNode> ProjectFiles { get; set; } = [];

    public ProjectInfoViewModel(NavigationService navigation, ProjectManifest project, UserConfig config)
    {
        _navigation = navigation;
        _project = project;
        _config = config;

        name = _project.Name;
        Description = _project.Description;
        UnityVersion = _project.UnityVersion;

        var templateIds = _project.TemplateIds;
        foreach(Guid id in templateIds)
        {
            projectTemplates.Add(TemplateService.GetTemplateById(config, id).Name);
        }

        ProjectFiles = TemplateService.BuildScriptTree(_project.Path, _project.Path);

    }

    public ProjectInfoViewModel()
    {
        name = "Mock Project";
        Description = "Description";
        UnityVersion = "6000.0";
        projectTemplates = ["Teste 1", "Teste 2", "Teste 3", "Teste 4", "Teste 5", "Teste 6", "Teste 7", "Teste 8"];
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
    private void OpenProjectFolder() => _navigation.Explorer.OpenBootStrapperFolder(_project.Path);

    [RelayCommand]
    private void UpdateProject()
    {
        _project.Name = name;
        _project.Description = Description;
        ProjectService.UpdateProject(_project);
    }
}
