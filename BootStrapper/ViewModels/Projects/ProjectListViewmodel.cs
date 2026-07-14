using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectListViewModel : ViewModelBase
{
    private readonly NavigationService _navigation  = null!;
    private readonly UserConfig _config  = null!;
    private readonly List<ProjectManifest> _projects  = null!;
    public ObservableCollection<ProjectManifest> ProjectList { get; set; }

    public ProjectListViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        _projects = ProjectService.ListProjects(config);
        ProjectList = new ObservableCollection<ProjectManifest>(_projects);
    }

    public ProjectListViewModel()
    {
        ProjectList = new ObservableCollection<ProjectManifest>
        {
            new ProjectManifest
            {
                Name = "Mock1",
                UnityVersion = "6000.0",
                Description = "Mock Description",
                UnityProjectPath = "",
                TemplateIds = [],
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid()
            },
            new ProjectManifest
            {
                Name = "Mock2",
                UnityVersion = "2022.3",
                Description = "Description 2",
                UnityProjectPath = "",
                TemplateIds = [],
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid()
            },
            new ProjectManifest
            {
                Name = "Mock3",
                UnityVersion = "2022.3",
                Description = "Third one",
                UnityProjectPath = "",
                TemplateIds = [],
                CreationDate = DateTime.Now,
                Id = Guid.NewGuid()
            }
        };
    }
    [RelayCommand] private async Task OpenUnity(ProjectManifest project)
    {
        await UnityService.OpenUnityProjectAsync(project);
    }

    [RelayCommand] private void GoToProjectCreate()
    {
        _navigation.CurrentView = new ProjectCreateViewModel(_navigation, _config);
    }

    [RelayCommand] private void GoToProjectInfo(ProjectManifest projectInfo)
    {
        _navigation.CurrentView = new ProjectInfoViewModel(_navigation, projectInfo, _config);
    }

    [RelayCommand] private void DeleteProject(ProjectManifest project)
    {
        UnityService.DeleteUnityProject(project);
        ProjectList.Remove(project);
    }
}
