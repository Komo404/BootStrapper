using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation = null!;
    private readonly TemplateManifest _template = null!;
    private readonly UserConfig _config = null!;

    public string Name { get; set; } = string.Empty;
    public string Desc { get; set; } = string.Empty;
    public string UnityV { get; set; } = string.Empty;
    public string MaxUnityV { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string TemplatePath { get; } = string.Empty;
    public List<string> Tags { get; set; } = new List<string>();

    public TemplateInfoViewModel(NavigationService navigation, TemplateManifest template, UserConfig config)
    {
        _navigation = navigation;
        _template = template;
        _config = config;

        Name = _template.Name;
        Desc = _template.Description;
        UnityV = _template.UnityVersion;
        MaxUnityV = _template.MaxUnityVersion;
        Author = _template.Author;
        Tags = _template.Tags;
        TemplatePath = _template.TemplatePath;
    }

    public TemplateInfoViewModel()
    {
        Tags = new List<string>
        {
            "ExampleTag1",
            "ExampleTag2"
        };
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
    private void OpenTemplateFolder() => _navigation.Explorer.OpenTemplateFolder(TemplatePath);

    [RelayCommand]
    private async Task DeleteTemplate()
    {
        await TemplateService.DeleteTemplate(_config, _template.Id);
        _navigation.CurrentView = new TemplateListViewModel(_navigation, _config);
    }
}
