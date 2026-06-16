using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly Project _project;
    private readonly UserConfig _config;

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string UVersion { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public ObservableCollection<TemplateNode> Templates { get; set; } = [];
    public string[] ProjectFiles { get; set; } = { };

    public ProjectInfoViewModel(NavigationService navigation, Project project, UserConfig config)
    {
        _navigation = navigation;
        _project = project; // Supondo que o project já foi carregado na HomeViewModel, então não precisa carregar de novo
        _config = config;

        Name = _project.Name;
        Description = _project.Description;
        UVersion = _project.UnityVersion;
        Author = _project.Author;
        Templates = _project.Templates;
    }

    public ProjectInfoViewModel()
    {
        Name = "Mock Project";
        Description = "Description";
        UVersion = "6000.0";
        Author = "Komori";
        Templates = new ObservableCollection<TemplateNode>
        {
            new TemplateNode
            {
                Name = "Teste"
            },
            new TemplateNode
            {
                Name = "Teste 2"
            },
            new TemplateNode
            {
                Name = "Teste 3"
            },
        };
    }
    private void GetProjectFiles()
    {
        var projectFiles = new List<string>();
    }

    [RelayCommand]
    private void OpenProject() => UnityService.OpenUnityProject(_config, _project);

    [RelayCommand]
    private void DeleteProject()
    {
        ProjectService.DeleteProject(_project);
        _navigation.CurrentView = new ProjectListViewModel(_navigation, _config);
    }
    [RelayCommand]
    private void AddTemplate(TemplateNode template)
    {

    }
    [RelayCommand]
    private void OpenProjectFolder() => ProjectService.OpenProjectFolder();

    [RelayCommand]
    private void UpdateProject()
    {

    }
}
