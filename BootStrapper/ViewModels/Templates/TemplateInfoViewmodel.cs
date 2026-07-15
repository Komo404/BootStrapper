using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation = null!;
    private readonly TemplateManifest _template = null!;
    private readonly UserConfig _config = null!;

    [ObservableProperty] public string name = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreationDate { get; } = new();
    [ObservableProperty] private TemplateCategory category;
    public ObservableCollection<string> AddedTags { get; set; } = [];
    public IReadOnlyList<string> Tags { get; } = ["2D", "3D", "Input System", "Cinemachine", "URP", "HDRP", "Addressables", "Localization", "NavMesh", "Mobile"];
    public IReadOnlyList<TemplateCategory> Categories { get; } = Enum.GetValues<TemplateCategory>();
    public ObservableCollection<string> AddedUnityVersions { get; set; } = [];
    public string ManualUnityVersion { get; set; } = string.Empty;
    public ObservableCollection<string> UnityVersions { get; } = [];
    public string UnityVersion { get; set; } = string.Empty;
    public string TemplatePath { get; } = string.Empty;

    [ObservableProperty] private ObservableCollection<TemplateNode> templateScripts = [];
    [ObservableProperty] private TemplateNode? selectedNode;

    public TemplateInfoViewModel(NavigationService navigation, TemplateManifest template, UserConfig config)
    {
        _navigation = navigation;
        _template = template;
        _config = config;

        name = _template.Name;
        Description = _template.Description;
        Category = _template.Category;
        AddedUnityVersions = new ObservableCollection<string>(_template.UnityVersions);
        CreationDate = _template.CreationDate;
        AddedTags = new ObservableCollection<string>(_template.Tags);
        TemplatePath = _template.TemplatePath;
        TemplateScripts = new ObservableCollection<TemplateNode>(ScriptTreeService.BuildScriptTree(TemplatePath, TemplatePath));


    }

    public TemplateInfoViewModel()
    {
        name = "Template Name";
        AddedUnityVersions = ["Teste", "Teste2"];
        AddedTags = ["E.Tag 1", "E.Tag 2", "E.Tag 3"];
        TemplateScripts = [
            new TemplateNode() {
                Name = "Teste", Children = [], IsFolder = true, RelativePath = "", UserScriptFolderPath = ""
            }];
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
    private void ChangeCategory(TemplateCategory newCategory)
    {
        Category = newCategory;
    }

    [RelayCommand]
    private void AddUnityVersion(string newUnityVersion)
    {
        if (!AddedUnityVersions.Contains(newUnityVersion)) AddedUnityVersions.Add(newUnityVersion);
    }

    [RelayCommand]
    private void RemoveUnityVersion(string newUnityVersion)
    {
        if (AddedUnityVersions.Contains(newUnityVersion)) AddedUnityVersions.Remove(newUnityVersion);
    }

    [RelayCommand]
    private void AddManualUnityVersion()
    {
        if (!string.IsNullOrWhiteSpace(ManualUnityVersion) && !AddedUnityVersions.Contains(ManualUnityVersion))
        {
            AddedUnityVersions.Add(ManualUnityVersion);
            ManualUnityVersion = string.Empty;
        }
    }

    [RelayCommand]
    private void AddTag(string tag)
    {
        if (!AddedTags.Contains(tag)) AddedTags.Add(tag);
    }

    [RelayCommand]
    private void RemoveTag(string tag)
    {
        if (AddedTags.Contains(tag)) AddedTags.Remove(tag);
    }

    [RelayCommand]
    private void AddScripts()
    {
        Task filesAdded = _navigation.Explorer.OpenFolderDialogAsync();
    }

    [RelayCommand]
    private void RemoveScript(TemplateNode selectedNode)
    {
        if (selectedNode is null) return;

        var relativePath = selectedNode.RelativePath;
        if (string.IsNullOrWhiteSpace(relativePath)) return;

        Debug.WriteLine(selectedNode);

        var fullPath = Path.Combine(TemplatePath, relativePath);

        if (selectedNode.IsFolder == true)
            Directory.Delete(fullPath, true);
        else
            File.Delete(fullPath);

        refreshTemplateScripts();
    }

    [RelayCommand]
    private void UpdateTemplate()
    {
        _template.Name = Name;
        _template.Description = Description;
        _template.Category = Category;
        _template.UnityVersions = AddedUnityVersions.ToList();
        _template.Tags = AddedTags.ToList();

        TemplateService.UpdateTemplateManifest(_template);
    }

    [RelayCommand]
    private void OpenTemplateFolder() => _navigation.Explorer.OpenBootStrapperFolder(TemplatePath);

    [RelayCommand]
    private async Task DeleteTemplate()
    {
        await TemplateService.DeleteTemplate(_config, _template.Id);
        _navigation.CurrentView = new TemplateListViewModel(_navigation, _config);
    }
    private void refreshTemplateScripts()
    {
        TemplateScripts = new ObservableCollection<TemplateNode>(ScriptTreeService.BuildScriptTree(TemplatePath, TemplatePath));
    }
}
