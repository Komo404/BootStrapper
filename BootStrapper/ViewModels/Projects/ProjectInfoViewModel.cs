using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly Project _project;
    private readonly UserConfig _config;

    public string Name { get; private set; }
    public string Description { get; private set; }
    public string UVersion { get; private set; }
    public string Author { get; private set; }
    public string[] Templates { get; private set; }

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

        Templates = new[]
        {
            "Template1",
            "Template2",
            "Template3"
        };
    }

    [RelayCommand]
    private void OpenProject() => UnityService.OpenUnityProject(_config, _project);

    [RelayCommand]
    private void DeleteProject() => ProjectService.DeleteProject(_project);

    [RelayCommand]
    private void OpenProjectFolder() => ProjectService.OpenProjectFolder();
}
