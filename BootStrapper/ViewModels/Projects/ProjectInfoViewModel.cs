using BootStrapper.Views;

namespace BootStrapper.ViewModels.Projects;

public partial class ProjectInfoViewModel : ViewModelBase
{
    private readonly NavigationService _navigation;
    public ProjectInfoViewModel(NavigationService navigation)
    {
        _navigation = navigation;
    }
}
