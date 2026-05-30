using BootStrapper.Core.Models;
using BootStrapper.Core.Service;
using BootStrapper.Views;
using CommunityToolkit.Mvvm.Input;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly Project _project;
    private readonly UserConfig _config;
    public ProjectInfoViewModel(NavigationService navigation, Project project, UserConfig config)
    {
        _navigation = navigation;
        _project = project; // Supondo que o projeto já foi carregado na HomeViewModel, então não precisa carregar de novo
        _config = config;
    }

    [RelayCommand]
    private void OpenProject() => UnityService.OpenUnityProject(_config, _project);

    [RelayCommand]
    private void DeleteProject() => ProjectService.DeleteProject(_project);

    [RelayCommand]
    private void OpenProjectFolder() => ProjectService.OpenProjectFolder();
}
