using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectListViewModel : ViewModelBase
{
    public string ProjectListMainTEXT { get; set; } = "Hello from PROJECT LIST";
    private readonly NavigationService _navigation;

    public ProjectListViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

    [RelayCommand]
    private void GoToHome()
    {
        _navigation.CurrentView = new HomeViewModel(_navigation);
    }
}
