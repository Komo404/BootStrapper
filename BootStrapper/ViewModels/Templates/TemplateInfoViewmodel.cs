using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly TemplateManifest _template;
    private readonly UserConfig _config;

    [ObservableProperty] public string name = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreationDate { get; } = new();
    [ObservableProperty] private TemplateCategory category;
    public ObservableCollection<string> AddedTags { get; set; } = [];
    public IReadOnlyList<string> Tags { get; } = ["2D", "3D", "Input System", "Cinemachine", "URP", "HDRP", "Addressables", "Localization", "NavMesh", "Mobile"];
    public IReadOnlyList<TemplateCategory> Categories { get; } = Enum.GetValues<TemplateCategory>();
    public ObservableCollection<string> AddedUnityVersions { get; set; } = [];
    public string ManualUnityVersion { get; set; } = string.Empty;
    public List<string> UnityVersions { get; set; } = [];
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
        TemplateScripts = TemplateService.BuildScriptTree(TemplatePath, TemplatePath);

        UnityVersions = UnityService.GetUnityVersions(_config);
    }

    public TemplateInfoViewModel()
    {
        name = "TEMPLATE NAME";
        AddedUnityVersions = ["Teste", "Teste2"];
        AddedTags = ["TagE 1", "TagE 2", "TagE 3"];
        TemplateScripts = [
            new TemplateNode()
            {
                Name = "Teste",
                Children = [],
                IsFolder = true,
                RelativePath = "",
                UserScriptFolderPath = ""
            }
            ];
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
        Debug.WriteLine(selectedNode);
        TemplateService.RemoveTreeNode(TemplateScripts, selectedNode);

            if(selectedNode.IsFolder == true)
                Directory.Delete(Path.Combine(TemplatePath, selectedNode.RelativePath), true);
            else {
                File.Delete(Path.Combine(TemplatePath, selectedNode.RelativePath));
            }

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
}
