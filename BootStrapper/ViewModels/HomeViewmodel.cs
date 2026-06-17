using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.ViewModels.Projects;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BootStrapper.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    public ObservableCollection<ProjectManifest> RecentProjects { get; set; }

    public HomeViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config ?? throw new ArgumentNullException(nameof(config));

        var projects = ProjectService.ListProjects(_config);
        RecentProjects = new ObservableCollection<ProjectManifest>(projects);
    }

    public HomeViewModel()
    {
        RecentProjects = new ObservableCollection<ProjectManifest>
        {
            new ProjectManifest
            {
                Name = "Mock1",
                UnityVersion = "6000f",
                Description = "Description",
                Author = "author",
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
                Author = "Komori",
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
                Author = "",
                Path = "",
                TemplateIds = [],
                ChangeHistory = [],
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid()
            }
        };
    }

    [RelayCommand]
    private void GoToProjectInfo(ProjectManifest project)
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");

        _navigation.CurrentView = new ProjectInfoViewModel(_navigation, project, _config);
    }

    [RelayCommand]
    private void OpenUnity(ProjectManifest project)
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");

        UnityService.OpenUnityProject(_config, project);
    }
}
