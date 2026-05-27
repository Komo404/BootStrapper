using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectCreateViewModel : ViewModelBase
{
    public string ProjectCreateMainTEXT { get; set; } = "Hello from PROJECT CREATE";
    private readonly NavigationService _navigation;
    private readonly BootStrapper.Core.Service.pRO _templateService;

    public ProjectCreateViewModel (NavigationService navigation)
    {
        _navigation = navigation;
    }

    [RelayCommand]
    private void GoToProjectCreate()
    {
        _navigation.CurrentView = new ProjectCreateViewModel(_navigation);
    }

    [RelayCommand]
    private void CreateProject()
    {
        _templateService.CreateProject(); // TODO
    }

}
