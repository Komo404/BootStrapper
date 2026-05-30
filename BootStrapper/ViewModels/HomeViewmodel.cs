using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.ViewModels.Projects;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace BootStrapper.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly NavigationService? _navigation;
    private readonly UserConfig? _config;
    public ObservableCollection<Project> RecentProjects { get; set; }

    public HomeViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config ?? throw new ArgumentNullException(nameof(config));

        var projects = ProjectService.ListProjects(_config);
        RecentProjects = new ObservableCollection<Project>(projects);
    }

    [RelayCommand]
    private void GoToProjectInfo(Project project)
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");

        _navigation.CurrentView = new ProjectInfoViewModel(_navigation, project, _config);
    }

    [RelayCommand]
    private void OpenUnity(Project project)
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");

        UnityService.OpenUnityProject(_config, project);
    }
}
