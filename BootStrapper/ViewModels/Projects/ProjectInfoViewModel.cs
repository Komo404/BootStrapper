using BootStrapper.Core.Models;
using BootStrapper.Views;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    private readonly Project _project;
    public ProjectInfoViewModel(NavigationService navigation, Project project)
    {
        _navigation = navigation;
        _project = project; // Supondo que o projeto já foi carregado na HomeViewModel, então não precisa carregar de novo
    }
}
