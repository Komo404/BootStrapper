using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Helpers;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    public string UserScriptFolderPath;
    public ObservableCollection<TemplateNode> TemplateScripts { get; } = []; // Preview
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = ["Camera", "Networking", "UI", "Save system", "Steam", "UI", "Animations","Action","Other"];
    public string Version { get; set; } = string.Empty;
    public string UnityVersion { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

    [ObservableProperty] private TemplateNode? selectedNode;

    public TemplateCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        TemplateScripts = new ObservableCollection<TemplateNode>();
    }

    public TemplateCreateViewModel()
    {
        TemplateScripts = new ObservableCollection<TemplateNode>()
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
    private async void AddUserScriptFolder()
    {
        UserScriptFolderPath = await _navigation.Explorer.OpenFolderDialogAsync();
    }

    [RelayCommand]
    private void RemoveScripts(ObservableCollection<TemplateNode> selectedNodes)
    {
        if (selectedNodes is null) return;

        foreach(var node in selectedNodes)
        {
            File.Delete(node.RelativePath);
        }
    }

    [RelayCommand]
    private void CreateTemplate(string UserScriptFolderPath)
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

        TemplateService.CreateTemplate(_config, newTemplateManifest, UserScriptFolderPath);

        _navigation.CurrentView = new TemplateInfoViewModel(_navigation, newTemplateManifest, _config);
    }

}
