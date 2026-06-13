using BootStrapper.Core.Models;
using BootStrapper.ViewModels.Projects;
using BootStrapper.ViewModels.Templates;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;

namespace BootStrapper.ViewModels;

public partial class SidebarViewModel: ViewModelBase
{
    private readonly NavigationService? _navigation;
    private readonly UserConfig? _config;

    public SidebarViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    [RelayCommand]
    private void GoToHome()
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new HomeViewModel(_navigation, _config);
    }
    [RelayCommand]
    private void GoToProjectCreate()
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new ProjectCreateViewModel(_navigation, _config);
    }
    [RelayCommand]
    private void GoToTemplateList()
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new TemplateListViewModel(_navigation, _config);
    }
    [RelayCommand]
    private void GoToProjectList()
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new ProjectListViewModel(_navigation, _config);
    }
    [RelayCommand]
    private void GoToCredits()
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new CreditsViewModel(_navigation);
    }

    [RelayCommand]
    private void GoToSettings()
    {
        if (_navigation is null)
            throw new InvalidOperationException("NavigationService não foi inicializado");
        _navigation.CurrentView = new SettingsViewModel(_navigation, _config);
    }
}
