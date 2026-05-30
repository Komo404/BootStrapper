using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly TemplateManifest _template;
    private readonly UserConfig _config;

    public string Name => _template.Name;
    public string Desc => _template.Description;
    public string UnityV => _template.UnityVersion;
    public string MaxUnityV => _template.MaxUnityVersion;
    public string Author => _template.Author;

    public TemplateInfoViewModel(NavigationService navigation, TemplateManifest template, UserConfig config)
    {
        _navigation = navigation;
        _template = template;
        _config = config;
    }

    [RelayCommand]
    private void UpdateTemplate()
    {
        TemplateManifest newTemplate = new TemplateManifest()
        {
            Id = _template.Id,
            Name = _template.Name,
            Description = _template.Description,
            Version = _template.Version,
            UnityVersion = _template.UnityVersion,
            MaxUnityVersion = _template.MaxUnityVersion,
            Author = _template.Author,
            ManifestPath = _template.ManifestPath,
            TemplatePath = _template.TemplatePath,
        };

        TemplateService.UpdateTemplateManifest(_config, _template);
    }

    [RelayCommand]
    private void OpenTemplateFolder() => TemplateService.OpenTemplateFolder();
}
