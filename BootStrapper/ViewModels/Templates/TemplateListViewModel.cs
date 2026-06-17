using BootStrapper.Core.Models;
using BootStrapper.Core.Services;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace BootStrapper.ViewModels.Templates;

public partial class TemplateListViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly UserConfig _config;
    private readonly List<TemplateManifest> _templates;

    public ObservableCollection<TemplateManifest> TemplateList { get; set; }

    public TemplateListViewModel(NavigationService navigation, UserConfig config)
    {
        _navigation = navigation;
        _config = config;
        _templates = TemplateService.GetAllTemplates(config);
        TemplateList = new ObservableCollection<TemplateManifest>(_templates);
    }

    public TemplateListViewModel()
    {
        TemplateList = new ObservableCollection<TemplateManifest>
        {
            new TemplateManifest
            {
                Name = "Mock1",
                Description = "Desc 1",
                Version = "1.0",
                UnityVersion = "2022",
                MaxUnityVersion = "2023",
                Author = "",
                Tags = [],
                TemplatePath = "",
                ManifestPath = "",
            },
            new TemplateManifest
            {
                Name = "Mock2",
                Description = "Desc 2",
                Version = "1.0",
                UnityVersion = "2022",
                MaxUnityVersion = "2023",
                Author = "",
                Tags = ["TAG 1", "TAG 2", "TAG 3"],
                TemplatePath = "C:",
                ManifestPath = "C:",
            },
        };
    }
    [RelayCommand]
    private void GoToTemplateCreate()
    {
        _navigation.CurrentView = new TemplateCreateViewModel(_navigation, _config);
    }

    [RelayCommand]
    private void GoToTemplateInfo(TemplateManifest template)
    {
        _navigation.CurrentView = new TemplateInfoViewModel(_navigation, template, _config);
    }

    [RelayCommand]
    private void ImportTemplate()
    {
        // Abre o file explorer, seleciona o arquivo, serializa ele)
    }

    [RelayCommand] 
    private async Task DeleteTemplate(TemplateManifest template)
    {
        if (template == null)
            throw new ArgumentNullException(nameof(template));

        await TemplateService.DeleteTemplate(_config, template.Id);
        TemplateList.Remove(template);
    }
}
