using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.ViewModels.Projects;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BootStrapper.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty]
    private string? preferredUnityVersion;

    [ObservableProperty]
    private bool autoLaunchProject;

    [ObservableProperty]
    private bool checkForUpdates;

    [ObservableProperty]
    private bool enableAnalytics;

    public string[] UnityVersions { get; set; }

    private readonly NavigationService _navigation;
    public ObservableCollection<ProjectManifest> RecentProjects { get; set; }

    public SettingsViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
    }

    [RelayCommand]
    private void SaveSettings()
    {

    }

    [RelayCommand]
    private void ResetSettings()
    {

    }
}
