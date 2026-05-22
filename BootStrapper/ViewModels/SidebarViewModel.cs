using BootStrapper.ViewModels.Projects;
using BootStrapper.ViewModels.Templates;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels;

public partial class SidebarViewModel: ViewModelBase
{
    private readonly NavigationService _navigation;

    public SidebarViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

    public SidebarViewModel() { }

    public string HomeView { get; set; } = "HomeView";
    public string ProjectCreate { get; set; } = "Project Create";
    public string TemplateView { get; set; } = "Template View";
    public string ProjectList { get; set; } = "Project List";
    public string Credits { get; set; } = "Credits";

    [RelayCommand]
    private void GoToHome()
    {
        _navigation.CurrentView = new HomeViewModel(_navigation);
    }
    [RelayCommand]
    private void GoToProjectCreate()
    {
        _navigation.CurrentView = new ProjectCreateViewModel(_navigation);
    }
    [RelayCommand]
    private void GoToTemplateInfo()
    {
        _navigation.CurrentView = new TemplateInfoViewModel(_navigation);
    }
    [RelayCommand]
    private void GoToProjectList()
    {
        _navigation.CurrentView = new ProjectListViewModel(_navigation);
    }
    [RelayCommand]
    private void GoToCredits()
    {
        _navigation.CurrentView = new ProjectCreateViewModel(_navigation);
    }
}
