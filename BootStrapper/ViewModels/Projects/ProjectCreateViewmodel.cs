using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.ViewModels.Templates;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectCreateViewModel : ViewModelBase
{
    private readonly NavigationService _navigation = null!;
    private readonly UserConfig _config = null!;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ChoseUnityVersion { get; set;  } = string.Empty;
    public ObservableCollection<string> UnityVersions { get; } = [];
    public ObservableCollection<TemplateManifest> Templates { get; set; } = [];
    public IReadOnlyList<string> Tags { get; } = ["2D", "3D", "Input System", "Cinemachine", "URP", "HDRP", "Addressables", "Localization", "NavMesh", "Mobile"];
    [ObservableProperty] private string actualFilter = string.Empty;
    public enum TemplateBrowsingMode { Official, Personal }
    [ObservableProperty] private TemplateBrowsingMode browsingMode = TemplateBrowsingMode.Official;
    public string BrowsingTitle => BrowsingMode == TemplateBrowsingMode.Official ? "Official Templates" : "My Templates";
    public ObservableCollection<TemplateManifest> BrowsableTemplates { get; set; } = [];
    public ObservableCollection<TemplateManifest> AddedTemplates { get; set; } = [];
    public List<Guid> AddedTemplateIds = [];
    [ObservableProperty] private TemplateManifest? selectedBrowseTemplate;

    public ProjectCreateViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        Templates = new ObservableCollection<TemplateManifest>(TemplateService.GetAllTemplates(_config));

        ChangeBrowserTemplates();
    }

    public ProjectCreateViewModel() {
        Templates = new ObservableCollection<TemplateManifest>()
        {
            new TemplateManifest
            {
                Name = "Mock1", Description = "",
                Category = new TemplateCategory(),
                UnityVersions = [],
                Tags = new List<string>(),
                TemplatePath = "",
                ManifestPath = "",
            },
            new TemplateManifest
            {
                Name = "Mock2",
                Description = "",
                Category = new TemplateCategory(),
                UnityVersions = [],
                Tags = new List<string>(),
                TemplatePath = "",
                ManifestPath = "",
            },
            new TemplateManifest
            {
                Name = "Mock3",
                Description = "",
                Category = new TemplateCategory(),
                UnityVersions = [],
                Tags = new List<string>(),
                TemplatePath = "",
                ManifestPath = "",
            }
        };
    }

    public bool IsLoadingUnityVersions { get; private set; }
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
    private void SetBrowsingMode(string mode)
    {
        BrowsingMode = mode == "Official" ? TemplateBrowsingMode.Official : TemplateBrowsingMode.Personal;
        ChangeBrowserTemplates();
        OnPropertyChanged(nameof(BrowsingTitle));
    }

    private void ChangeBrowserTemplates()
    {
        List<TemplateManifest> officialTemplates = TemplateService.GetAllTemplates(_config);

        BrowsableTemplates.Clear();
        if (BrowsingMode == TemplateBrowsingMode.Official)
        {
            foreach (var template in officialTemplates)
                BrowsableTemplates.Add(template);
        } else
        {
            return;
        }

        for (int i = 0; i < BrowsableTemplates.Count(); i++)
        {
            var templateManifest = BrowsableTemplates[i];
            if (ChoseUnityVersion != string.Empty) {
                if (!templateManifest.UnityVersions.Contains(ChoseUnityVersion))
                {
                    BrowsableTemplates.Remove(templateManifest);
                }
            }
        }
    }

    [RelayCommand]
    private void GoToTemplateCreate()
    {
        _navigation.CurrentView = new TemplateCreateViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void SelectTemplate(TemplateManifest template) => SelectedBrowseTemplate = template;

    [RelayCommand]
    private void RemoveTemplate(TemplateManifest template) => AddedTemplates.Remove(template);

    [RelayCommand]
    private void AddSelectedTemplateToProject()
    {
        if (SelectedBrowseTemplate is null) return;
        if (!AddedTemplates.Contains(SelectedBrowseTemplate))
            AddedTemplates.Add(SelectedBrowseTemplate);
    }

    [RelayCommand]
    private async Task CreateProject(ProjectManifest projectInfo)
    {
        foreach (var template in AddedTemplates)
        {
            AddedTemplateIds.Add(template.Id);
        };

        string unityProjectsFolderPath = _config.UnityProjectsFolderPath;

        string cleanedName = string.Join("_", Name.Split(Path.GetInvalidFileNameChars()));

        ProjectManifest newProject = new ProjectManifest
        {
            Id = Guid.NewGuid(),
            Name = Name,
            Description  = Description,
            CreationDate = DateTime.Now,
            UnityProjectPath = Path.Combine(unityProjectsFolderPath, cleanedName),
            UnityVersion = ChoseUnityVersion,
            TemplateIds = AddedTemplateIds
        };

        await UnityService.CreateUnityProjectAsync(newProject);
        ProjectService.CreateProjectManifestAndCopyTemplates(_config, newProject);
        _navigation.CurrentView = new ProjectInfoViewModel(_navigation, newProject, _config);
    }

}
