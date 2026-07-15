
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
    private readonly NavigationService _navigation = null!;
    private readonly UserConfig _config = null!;
    public string UserScriptFolderPath = string.Empty;

    [ObservableProperty] private ObservableCollection<TemplateNode> templateScripts = [];
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    [ObservableProperty] private TemplateCategory category;
    public ObservableCollection<string> AddedTags { get; set; } = [];
    public IReadOnlyList<TemplateCategory> Categories { get; } = Enum.GetValues<TemplateCategory>();
    public IReadOnlyList<string> Tags { get; } = [ "2D", "3D", "Input System", "Cinemachine", "URP", "HDRP", "Addressables", "Localization", "NavMesh", "Mobile"];

    public string UnityVersion { get; set; } = string.Empty;
    public ObservableCollection<string> SelectedUnityVersions { get; set; } = [];
    public string ManualUnityVersion { get; set; } = string.Empty;
    public ObservableCollection<string> UnityVersions { get; } = [];
    public string Author { get; set; } = string.Empty;

    [ObservableProperty] private TemplateNode? selectedNode;

    public TemplateCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;



    }

    public TemplateCreateViewModel()
    {
        UnityVersions.Add("Ver1");
        UnityVersions.Add("Ver2");
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

    [ObservableProperty] private bool isLoadingUnityVersions;
    [ObservableProperty] private string unityVersionsStatus = string.Empty;

    [RelayCommand]
    private async Task LoadUnityVersions()
    {
        if (IsLoadingUnityVersions) return;
        IsLoadingUnityVersions = true;
        UnityVersionsStatus = "Loading Unity Editors…";
        try
        {
            var versions = await UnityService.GetUnityVersionsAsync();
            UnityVersions.Clear();
            foreach (string version in versions) UnityVersions.Add(version);
            UnityVersionsStatus = versions.Count == 0 ? "No installed Unity Editor found." : string.Empty;
        }
        catch (Exception ex)
        {
            UnityVersionsStatus = ex.Message;
        }
        finally
        {
            IsLoadingUnityVersions = false;
        }
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
        if (string.IsNullOrWhiteSpace(scripts)) return;

        UserScriptFolderPath = scripts;

        TemplateScripts = new ObservableCollection<TemplateNode>(ScriptTreeService.BuildScriptTree(UserScriptFolderPath, UserScriptFolderPath));
    }

    [RelayCommand]
    private void RemoveScript(TemplateNode selectedNode)
    {
        if (selectedNode is null) return;
        var scripts = TemplateScripts.ToList();
        if (ScriptTreeService.RemoveTreeNode(scripts, selectedNode))
        {
            // Rebind the tree after editing the selection, without reading discarded files back from disk.
            TemplateScripts = [];
            TemplateScripts = new ObservableCollection<TemplateNode>(scripts);
        }
    }

    [RelayCommand]
    private void CreateTemplate()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description) || UnityVersions == null || AddedTags == null || TemplateScripts == null)
        {
            return;
        }

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

        var createdTemplate = TemplateService.CreateTemplate(_config, newTemplateManifest, new List<TemplateNode>(TemplateScripts));

        _navigation.CurrentView = new TemplateInfoViewModel(_navigation, createdTemplate, _config);
    }

}
