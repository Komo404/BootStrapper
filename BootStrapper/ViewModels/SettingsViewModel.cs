using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.ViewModels.Projects;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly UserConfig _config;


    [ObservableProperty] private bool autoLaunchProject;
    [ObservableProperty] private bool checkForUpdates;
    [ObservableProperty] private string customUnityEditorsPath;
    [ObservableProperty] private string customUnityProjectsFolderPath;

    private readonly NavigationService _navigation;

    public string BootStrapperPath { get; set; } = string.Empty;
    public string ProjectsPath { get; set; } = string.Empty;
    public string TemplatesPath { get; set; } = string.Empty;

    public SettingsViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        CustomUnityEditorsPath = config.CustomUnityEditorsPath;
        CustomUnityProjectsFolderPath = config.CustomUnityProjectsFolderPath;
        AutoLaunchProject = config.AutoLaunchProject;
        CheckForUpdates = config.AutoUpdateEnabled;

        ProjectsPath = config.ProjectsFolder;
        TemplatesPath = config.TemplatesFolder;
        BootStrapperPath = Path.GetDirectoryName(TemplatesPath);
    }

    public SettingsViewModel() { }

    [RelayCommand]
    private async Task ChangeUnityEditorPath()
    {
        string? newUnityEditorPath = await _navigation.Explorer.OpenFolderDialogAsync();
        if (newUnityEditorPath != null)
            CustomUnityEditorsPath = newUnityEditorPath;
        SaveSettings();
    }

    [RelayCommand]
    private async Task ChangeUnityProjectsFolderPath()
    {
        string? newUnityProjectsFolderPath = await _navigation.Explorer.OpenFolderDialogAsync();
        if (newUnityProjectsFolderPath != null)
            CustomUnityProjectsFolderPath = newUnityProjectsFolderPath;
        SaveSettings();
    }

    [RelayCommand]
    private void SaveSettings()
    {
        _config.AutoLaunchProject = AutoLaunchProject;
        _config.AutoUpdateEnabled = CheckForUpdates;
        _config.CustomUnityEditorsPath = CustomUnityEditorsPath;
        ConfigService.SaveConfig(Path.Combine(BootStrapperPath, "config.json"), _config);
    }

    [RelayCommand]
    private void ResetSettings()
    {
        ConfigService.CreateDefaultConfig(Path.Combine(BootStrapperPath, "config.json"));
        var newConfig = ConfigService.LoadConfig(Path.Combine(BootStrapperPath, "config.json"));

        _config.AutoLaunchProject = newConfig.AutoLaunchProject;
        _config.AutoUpdateEnabled = newConfig.AutoUpdateEnabled;
        _config.CustomUnityEditorsPath = newConfig.CustomUnityEditorsPath;

        RefreshUI(newConfig);
    }

    private void RefreshUI(UserConfig newConfig)
    {
        CustomUnityEditorsPath = newConfig.CustomUnityEditorsPath;
        AutoLaunchProject = newConfig.AutoLaunchProject;
        CheckForUpdates = newConfig.AutoUpdateEnabled;
    }
}
