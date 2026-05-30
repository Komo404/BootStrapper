using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;

    public string Name { get; set; }
    public string Description { get; set; }
    public string Version { get; set; } 
    public string UnityVersion { get; set; }

    public TemplateCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;
    }

    [RelayCommand]
    private void GoToHome()
    {
        _navigation.CurrentView = new HomeViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void CreateTemplate(TemplateManifest template)
    {
        var newTemplateManifest = new TemplateManifest
        {
            Id = Guid.Empty,
            Name = Name,
            Description = Description,
            Version = Version,
            UnityVersion = UnityVersion,
            Tags = new List<string>(),
            TemplatePath = string.Empty,
            ManifestPath = string.Empty
        };

        TemplateService.CreateTemplate(_config, newTemplateManifest);
        _navigation.CurrentView = new TemplateInfoViewModel(_navigation, newTemplateManifest, _config);
    }
}
