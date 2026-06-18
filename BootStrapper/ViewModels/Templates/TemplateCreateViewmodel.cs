using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Logging;
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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    public string UserScriptFolderPath;

    [ObservableProperty]
    private ObservableCollection<TemplateNode> templateScripts = []; // Preview. OBS: ObservableProperty: temp = algo; ObservableCollection = .add/.remove
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [ObservableProperty] private TemplateCategory category;
    public IReadOnlyList<TemplateCategory> Categories { get; } = Enum.GetValues<TemplateCategory>();
    public ObservableCollection<string> AddedTags { get; set; } = [];
    public IReadOnlyList<string> Tags { get; } = [ "2D", "3D", "Input System", "Cinemachine", "URP", "HDRP", "Addressables", "Localization", "NavMesh", "Mobile"];
    public string UnityVersion { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;

    [ObservableProperty] private TemplateNode? selectedNode;

    public TemplateCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;
    }

    public TemplateCreateViewModel()
    {
        AddedTags = ["Camera"];
        TemplateScripts = new ObservableCollection<TemplateNode>()
        {
            new TemplateNode {
                Name = "ExampleScript.cs",
                UserScriptFolderPath = "",
                RelativePath = "Assets/Scripts/ExampleScript.cs",
                IsFolder = false,
                Children =
                [
                    new TemplateNode
                    {
                        Name = "ChildScript.cs",
                        UserScriptFolderPath = "",
                        RelativePath = "Assets/Scripts/ChildScript.cs",
                        IsFolder = false,
                        Children = []
                    }
                ]
            },
            new TemplateNode {
                Name = "ExampleFolder",
                UserScriptFolderPath = "",
                RelativePath = "Assets/ExampleFolder",
                IsFolder = true,
                Children =
                [
                    new TemplateNode
                    {
                        Name = "NestedScript.cs",
                        UserScriptFolderPath = "",
                        RelativePath = "Assets/ExampleFolder/NestedScript.cs",
                        IsFolder = false,
                        Children = []
                    }
                ]
            }
        };
    }

    [RelayCommand]
    private void GoToHome()
    {
        _navigation.CurrentView = new HomeViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void SelectCategory(TemplateCategory category)
    {
        Category = category;
    }

    [RelayCommand]
    private void AddTag(string tag)
    {
        if (!AddedTags.Contains(tag)) AddedTags.Add(tag);
    }

    [RelayCommand]
    private void RemoveTag(string tag)
    {
        AddedTags.Remove(tag);
    }

    [RelayCommand]
    private async Task AddUserScriptFolder()
    {
        var scripts = await _navigation.Explorer.OpenFolderDialogAsync();

        UserScriptFolderPath = scripts;

        TemplateScripts = TemplateService.BuildScriptTree(UserScriptFolderPath, UserScriptFolderPath);
    }

    [RelayCommand]
    private void RemoveScript(TemplateNode selectedNode)
    {
        if (selectedNode is null) return;
        TemplateService.RemoveTreeNode(TemplateScripts, selectedNode);
    }

    [RelayCommand]
    private void CreateTemplate()
    {
        var newTemplateManifest = new TemplateManifest
        {
            Id = Guid.Empty,
            Name = Name,
            Description = Description,
            Category = Category,
            Version = "1.0",
            UnityVersion = UnityVersion,
            Tags = AddedTags.ToList(),
            Author = Author,
            TemplatePath = string.Empty,
            ManifestPath = string.Empty
        };

        var createdTemplate = TemplateService.CreateTemplate(_config, newTemplateManifest, TemplateScripts);

        _navigation.CurrentView = new TemplateInfoViewModel(_navigation, createdTemplate, _config);
    }

}
