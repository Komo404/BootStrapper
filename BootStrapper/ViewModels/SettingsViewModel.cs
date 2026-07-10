using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    [ObservableProperty] private bool autoLaunchProject;
    [ObservableProperty] private bool checkForUpdates;
    [ObservableProperty] private string unityProjectsFolderPath = null!;

    private readonly NavigationService _navigation = null!;
    private readonly UserConfig _config = null!;
    public string BootStrapperPath { get; set; } = string.Empty;
    public string TemplatesPath { get; set; } = string.Empty;
    public string ProjectsPath { get; set; } = string.Empty;

    public SettingsViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        UnityProjectsFolderPath = config.UnityProjectsFolderPath;
        AutoLaunchProject = config.AutoLaunchProject;
        CheckForUpdates = config.AutoUpdateEnabled;
        TemplatesPath = config.TemplatesFolderPath;
        BootStrapperPath = config.BootStrapperFolderPath;
        ProjectsPath = config.UnityProjectsFolderPath;
    }

    public SettingsViewModel() { }

    [RelayCommand]
    private async Task ChangeUnityProjectsFolderPath()
    {
        string? newUnityProjectsFolderPath = await _navigation.Explorer.OpenFolderDialogAsync();
        if (newUnityProjectsFolderPath != null)
            UnityProjectsFolderPath = newUnityProjectsFolderPath;
        else
            UnityProjectsFolderPath = _config.UnityProjectsFolderPath;
        SaveSettings();
    }

    [RelayCommand]
    private void SaveSettings()
    {
        _config.AutoLaunchProject = AutoLaunchProject;
        _config.AutoUpdateEnabled = CheckForUpdates;
        _config.UnityProjectsFolderPath = UnityProjectsFolderPath;
        ConfigService.SaveConfig(App.ConfigFilePath, _config);
    }

    [RelayCommand]
    private void ResetSettings()
    {
        ConfigService.CreateDefaultConfig(App.ConfigFilePath);
        var newResetedConfig = ConfigService.LoadConfig(App.ConfigFilePath);

        _config.AutoLaunchProject = newResetedConfig.AutoLaunchProject;
        _config.AutoUpdateEnabled = newResetedConfig.AutoUpdateEnabled;
        _config.UnityProjectsFolderPath = newResetedConfig.UnityProjectsFolderPath;

        RefreshUI(newResetedConfig);
    }

    private void RefreshUI(UserConfig newConfig)
    {
        UnityProjectsFolderPath = newConfig.UnityProjectsFolderPath;
        AutoLaunchProject = newConfig.AutoLaunchProject;
        CheckForUpdates = newConfig.AutoUpdateEnabled;
    }
}
