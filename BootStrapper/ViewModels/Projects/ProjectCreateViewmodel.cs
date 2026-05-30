using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    public string Name { get; set; }
    public string Description { get; set; }
    public string Path { get; set; }
    public string UnityVersion { get; set; }
    public string[] Tags { get; set; }
    public string[] Templates { get; set; }
    public string Author { get; set; }

    public ProjectCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;
    }

    [RelayCommand]
    private void GoToProjectCreate()
    {
        _navigation.CurrentView = new ProjectCreateViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void CreateProject(Project projectInfo)
    {
        Project newProject = new Project
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Description  = Description,
            CreationDate = DateTime.Now,
            Path = Path,
            UnityVersion = UnityVersion,
            Author = Author,
            Templates = Templates,
            ChangeHistory = Array.Empty<string>()
        };

        ProjectService.CreateProject(_config, newProject);
        _navigation.CurrentView = new ProjectInfoViewModel(_navigation, newProject, _config);
    }

}
