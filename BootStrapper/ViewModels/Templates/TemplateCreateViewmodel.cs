
using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    public string UserScriptFolderPath;

    [ObservableProperty]
    private ObservableCollection<TemplateNode> templateScripts = [];
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [ObservableProperty] private TemplateCategory category;
    public ObservableCollection<string> AddedTags { get; set; } = [];
    public IReadOnlyList<TemplateCategory> Categories { get; } = Enum.GetValues<TemplateCategory>();
    public IReadOnlyList<string> Tags { get; } = [ "2D", "3D", "Input System", "Cinemachine", "URP", "HDRP", "Addressables", "Localization", "NavMesh", "Mobile"];

    public string UnityVersion { get; set; } = String.Empty;
    public ObservableCollection<string> SelectedUnityVersions { get; set; } = [];
    public string ManualUnityVersion { get; set; } = string.Empty;
    public List<string> UnityVersions { get; set; } = [];
    public string Author { get; set; } = string.Empty;

    [ObservableProperty] private TemplateNode? selectedNode;

    public TemplateCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        UnityVersions = UnityService.GetUnityVersions(_config);
    }

    public TemplateCreateViewModel()
    {
        UnityVersions = ["Ver1", "Ver2"];
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
    private void AddUnityVersion(string version)
    {
        if (!SelectedUnityVersions.Contains(version))
            SelectedUnityVersions.Add(version);
    }

    [RelayCommand]
    private void RemoveUnityVersion(string version)
    {
        SelectedUnityVersions.Remove(version);
    }


    [RelayCommand]
    private void AddManualUnityVersion()
    {
        if (!string.IsNullOrWhiteSpace(ManualUnityVersion) && !SelectedUnityVersions.Contains(ManualUnityVersion))
        {
            SelectedUnityVersions.Add(ManualUnityVersion);
            ManualUnityVersion = string.Empty;
        }
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
        //if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || UnityVersions == null || AddedTags == null || TemplateScripts == null)  return;

        var newTemplateManifest = new TemplateManifest
        {
            Id = Guid.Empty,
            TemplatePath = string.Empty,
            ManifestPath = string.Empty,

            Name = Name,
            Description = Description,
            Category = Category,
            UnityVersions = SelectedUnityVersions.ToList(),
            Tags = AddedTags.ToList()
        };

        var createdTemplate = TemplateService.CreateTemplate(_config, newTemplateManifest, TemplateScripts);

        _navigation.CurrentView = new TemplateInfoViewModel(_navigation, createdTemplate, _config);
    }

}
