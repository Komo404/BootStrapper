using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;

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
    }

}
