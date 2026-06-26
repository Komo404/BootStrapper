using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.ViewModels.Projects;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BootStrapper.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    [ObservableProperty] private ObservableCollection<ProjectManifest> projects = [];

    public HomeViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config ?? throw new ArgumentNullException(nameof(config));

        List<ProjectManifest> projectlist = ProjectService.ListProjects(_config);
        projects = new ObservableCollection<ProjectManifest>(projectlist);
    }

    public HomeViewModel()
    {
        Projects = new ObservableCollection<ProjectManifest>
        {
            new ProjectManifest
            {
                Name = "Mock1",
                UnityVersion = "6000f",
                Description = "Description",
                Path = "C:",
                TemplateIds = [],
                ChangeHistory = [],
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid()
            },
            new ProjectManifest
            {
                Name = "Mock2",
                UnityVersion = "UV",
                Description = "DESC",
                Path = "/path",
                TemplateIds = [],
                ChangeHistory = [],
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid()
            },
            new ProjectManifest
            {
                Name = "Mock3",
                UnityVersion = "",
                Description = "",
                Path = "",
                TemplateIds = [],
                ChangeHistory = [],
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid()
            }
        };
    }

    [RelayCommand]
    private void OpenUnity(ProjectManifest project)
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");

        UnityService.OpenUnityProject(_config, project);
    }

    [RelayCommand]
    private void GoToProjectInfo(ProjectManifest project)
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");

        _navigation.CurrentView = new ProjectInfoViewModel(_navigation, project, _config);
    }

    [RelayCommand]
    private void DeleteProject(ProjectManifest project)
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");

        ProjectService.DeleteProject(project);
        projects.Remove(project);
    }
}
