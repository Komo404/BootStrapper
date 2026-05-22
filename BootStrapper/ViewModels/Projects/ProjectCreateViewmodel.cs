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

    public ProjectCreateViewModel (NavigationService navigation)
    {
        _navigation = navigation;
    }

    [RelayCommand]
    private void GoToHome()
    {
        _navigation.CurrentView = new HomeViewModel(_navigation);
    }
}
