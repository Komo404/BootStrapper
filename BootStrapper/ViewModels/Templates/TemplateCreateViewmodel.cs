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
    public List<TemplateNode> TemplateScripts {  get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = [];
    public string Version { get; set; } = string.Empty;
    public string UnityVersion { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

    public TemplateCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        TemplateScripts = new List<TemplateNode>();
    }

    public TemplateCreateViewModel()
    {
        TemplateScripts = new List<TemplateNode>()
        {
            new TemplateNode
            {
                Name = "ExampleScript.cs",
                RelativePath = "Assets/Scripts/ExampleScript.cs",
                IsFolder = false,
                Children = new List<TemplateNode>()
                {
                    new TemplateNode
                    {
                        Name = "ChildScript.cs",
                        RelativePath = "Assets/Scripts/ChildScript.cs",
                        IsFolder = false,
                        Children = new List<TemplateNode>()
                    }
                }
            },
            new TemplateNode
            {
                Name = "ExampleFolder",
                RelativePath = "Assets/ExampleFolder",
                IsFolder = true,
                Children = new List<TemplateNode>()
                {
                    new TemplateNode
                    {
                        Name = "NestedScript.cs",
                        RelativePath = "Assets/ExampleFolder/NestedScript.cs",
                        IsFolder = false,
                        Children = new List<TemplateNode>()
                    }
                }
            }
        };

        Tags = new List<string>()
        {
            "ExampleTag1",
            "ExampleTag2",
            "ExampleTag3"
        };

    }

    [RelayCommand]
    private void GoToHome()
    {
        _navigation.CurrentView = new HomeViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void AddTemplateScript()
    {
        // Logic to add a template script to the TemplateScripts list
        // This could involve opening a file dialog to select a script, for example
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
            Tags = Tags,
            Author = Author,
            TemplatePath = string.Empty,
            ManifestPath = string.Empty
        };

        TemplateService.CreateTemplate(_config, newTemplateManifest);
        _navigation.CurrentView = new TemplateInfoViewModel(_navigation, newTemplateManifest, _config);
    }

}
