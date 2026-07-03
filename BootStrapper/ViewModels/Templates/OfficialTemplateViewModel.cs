using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace BootStrapper.ViewModels.Templates;

public partial class OfficialTemplatesViewModel : ViewModelBase
{
    public NavigationService _navigation;
    public UserConfig _config;

    public ObservableCollection<TemplateManifest> Templates { get; set; } = [];
    [ObservableProperty] private TemplateManifest? selectedTemplate;

    bool IsInstalled = false;
    public string ActionLabel => IsInstalled == true ? "Add to Project" : "Download";

    public OfficialTemplatesViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;

        Templates = new ObservableCollection<TemplateManifest>(TemplateService.GetAllTemplates(config));
    }

    public OfficialTemplatesViewModel()
    {
        _navigation = null!;
        _config = null!;

        Templates = 
        [
            new TemplateManifest()
            {
                Name = "Hsm Machine",
                CreationDate = new DateTime(),
                Category = new TemplateCategory(),
                ManifestPath = "",
                TemplatePath = "",
                Description = "Description DescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescriptionDescription",
                Tags = ["teste", "teste2", "tag3"],
                UnityVersions = ["1.0.0f"]
            },
            new TemplateManifest()
            {
                Name = "Hsm Machine",
                CreationDate = new DateTime(),
                Category = new TemplateCategory(),
                ManifestPath = "",
                TemplatePath = "",
                Description = "Description",
                Tags = ["teste", "teste2", "tag3"],
                UnityVersions = ["1.0.0f"]
            },
            new TemplateManifest()
            {
                Name = "Hsm Machine",
                CreationDate = new DateTime(),
                Category = new TemplateCategory(),
                ManifestPath = "",
                TemplatePath = "",
                Description = "Description",
                Tags = ["teste", "teste2", "tag3"],
                UnityVersions = ["1.0.0f"]
            },
        ];
    }

    [RelayCommand]
    private void SelectTemplate(TemplateManifest template) => SelectedTemplate = template;

    [RelayCommand]
    private void Action()
    {
        // Download or Add to Project, depending on IsInstalled
    }
}
