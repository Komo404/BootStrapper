using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.Views;
using BootStrapper.Views.Projects;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectListViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    private readonly List<Project> _projects;
    public ObservableCollection<Project> ProjectList { get; set; }

    public ProjectListViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        _projects = ProjectService.ListProjects(config);
        ProjectList = new ObservableCollection<Project>(_projects);
    }

    [RelayCommand]
    private void GoToProjectInfo(Project project)
    {
        _navigation.CurrentView = new ProjectInfoViewModel(_navigation, project, _config);
    }

    [RelayCommand]
    private void DeleteProject(Project project)
    {
        ProjectService.DeleteProject(project);
    }
}
