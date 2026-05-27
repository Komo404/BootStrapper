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
    private readonly NavigationService? _navigation;

    public SidebarViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }

    public SidebarViewModel() { }

    [RelayCommand]
    private void GoToHome()
    {
        Console.WriteLine("GoToHome");
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new HomeViewModel(_navigation);
    }
    [RelayCommand]
    private void GoToProjectCreate()
    {
        Console.WriteLine("GoTo2");
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new ProjectCreateViewModel(_navigation);
    }
    [RelayCommand]
    private void GoToTemplateList()
    {
        Console.WriteLine("GoTo3");
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new TemplateListViewModel(_navigation);
    }
    [RelayCommand]
    private void GoToProjectList()
    {
        Console.WriteLine("GoTo4");
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new ProjectListViewModel(_navigation);
    }
    [RelayCommand]
    private void GoToCredits()
    {
        Console.WriteLine("GoTo5");
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new CreditsViewModel(_navigation);
    }
}
