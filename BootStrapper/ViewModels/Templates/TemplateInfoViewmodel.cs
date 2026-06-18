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

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TemplateCategory Category { get; set; }
    public ObservableCollection<string> AddedTags { get; set; } = [];
    public DateTime CreationDate { get; } = new();
    public IReadOnlyList<string> Tags { get; } = ["2D", "3D", "Input System", "Cinemachine", "URP", "HDRP", "Addressables", "Localization", "NavMesh", "Mobile"];
    public string Version { get; set; } = string.Empty;
    public string UnityVersion { get; set; } = string.Empty;
    public string MaxUnityVersion { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string TemplatePath { get; } = string.Empty;

    [ObservableProperty] private ObservableCollection<TemplateNode> templateScripts = [];
    [ObservableProperty] private TemplateNode? selectedNode;

    public TemplateInfoViewModel(NavigationService navigation, TemplateManifest template, UserConfig config)
    {
        _navigation = navigation;
        _template = template;
        _config = config;

        Name = _template.Name;
        Description = _template.Description;
        Version = _template.Version;
        Category = _template.Category;
        UnityVersion = _template.UnityVersion;
        MaxUnityVersion = _template.MaxUnityVersion;
        CreationDate = _template.CreationDate;
        Author = _template.Author;
        AddedTags = new ObservableCollection<string>(_template.Tags);
        TemplatePath = _template.TemplatePath;
        TemplateScripts = TemplateService.BuildScriptTree(TemplatePath, TemplatePath);
    }

    public TemplateInfoViewModel()
    {
        Name = "TEMPLATE NAME";
        Version = "1.0";
        UnityVersion = "1.0.0ff";
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
    private void AddTag(string tag)
    {
        if (!AddedTags.Contains(tag)) AddedTags.Add(tag);
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
                Directory.Delete(Path.Combine(TemplatePath, selectedNode.RelativePath, selectedNode.Name));
            else
            {
                File.Delete(Path.Combine(TemplatePath, selectedNode.RelativePath, selectedNode.Name));
            }

    }

    [RelayCommand]
    private void UpdateTemplate()
    {
        _template.Name = Name;
        _template.Description = Description;
        _template.Category = Category;
        _template.Version = Version;
        _template.UnityVersion = UnityVersion;
        _template.Tags = AddedTags.ToList();
        _template.MaxUnityVersion = MaxUnityVersion;
        _template.Author = Author;

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
