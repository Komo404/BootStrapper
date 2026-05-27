using BootStrapper.ViewModels.Projects;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    public string HomeViewMainTEXT { get; set; } = "Hello from HomeView";
    private readonly NavigationService? _navigation;

    public HomeViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

    public HomeViewModel() { }

    [RelayCommand]
    private void GoToProjectCreate()
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");

        _navigation.CurrentView = new ProjectCreateViewModel(_navigation);
    }
}
